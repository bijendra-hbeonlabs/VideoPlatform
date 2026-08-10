#!/usr/bin/env python3
"""
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  HBeonLabs Video Display Platform
  Hardware Device Agent  v3.0  (FINAL — PRODUCTION READY)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

This is the SINGLE entry point for all hardware devices.
Run this script and everything happens automatically:

  1. Scans ./media/videos/ for .mp4/.avi/.mkv files
  2. Starts a local HTTP server on port 8765
     → Serves player.html (full-screen browser video player)
     → Streams video files with range-request support (seek)
     → Exposes /playlist, /identity, /status REST APIs
  3. Connects to MQTT broker  66.116.227.217:1883
     → User: admin / Password: admin123
     → Publishes telemetry (CPU, RAM, Temp, video state) every 5s
     → Subscribes to commands: PLAY, PAUSE, NEXT, STOP, REBOOT etc.
  4. Launches Chromium/Chrome browser in kiosk fullscreen mode
     → Points to http://localhost:8765/
     → Player auto-plays videos sequentially with timer
     → Loops playlist and replays automatically
  5. Device is identified by MAC address + hostname (unique HW-ID)

Usage:
  python device_agent.py                     ← auto-detect everything
  python device_agent.py --device-id 1       ← set specific device ID
  python device_agent.py --no-browser        ← headless mode (no display)

Install deps first:
  pip install paho-mqtt psutil requests
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
"""

import argparse
import hashlib
import json
import logging
import mimetypes
import os
import platform
import re
import signal
import socket
import subprocess
import sys
import threading
import time
import uuid
from datetime import datetime, timezone
from http.server import BaseHTTPRequestHandler, HTTPServer
from pathlib import Path
from urllib.parse import urlparse

# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# OPTIONAL DEPENDENCIES
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
try:
    import paho.mqtt.client as mqtt
except ImportError:
    print("\n[ERROR] paho-mqtt not installed.")
    print("  Run: pip install paho-mqtt psutil requests\n")
    sys.exit(1)

try:
    import psutil
    HAS_PSUTIL = True
except ImportError:
    HAS_PSUTIL = False

# Try importing dotenv
try:
    from dotenv import load_dotenv
    env_file = Path(__file__).parent / ".env"
    if env_file.exists():
        load_dotenv(dotenv_path=env_file)
except ImportError:
    pass

# Try importing db_manager
sys.path.insert(0, str(Path(__file__).parent.resolve()))
try:
    from db_manager import HardwareDBManager
    HAS_DB_MANAGER = True
except ImportError:
    HAS_DB_MANAGER = False

# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# CONSTANTS & ENV CONFIG
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
BASE_DIR       = Path(__file__).parent.resolve()
MEDIA_DIR      = BASE_DIR / "media" / "videos"
PLAYER_HTML    = BASE_DIR / "player.html"
LOG_FILE       = BASE_DIR / "device_agent.log"
LOCAL_PORT     = int(os.environ.get("VDP_LOCAL_PORT", "8765"))
HEARTBEAT_SEC  = 5
VIDEO_EXTS     = {".mp4", ".avi", ".mkv", ".mov", ".webm", ".ts", ".m4v", ".flv", ".wmv"}

MQTT_BROKER    = os.environ.get("VDP_MQTT_BROKER",  "66.116.227.217")
MQTT_PORT      = int(os.environ.get("VDP_MQTT_PORT", "1883"))
MQTT_USER      = os.environ.get("VDP_MQTT_USER",    "admin")
MQTT_PASS      = os.environ.get("VDP_MQTT_PASS",    "admin123")

DB_HOST        = os.environ.get("MARIADB_HOST", "localhost")
DB_PORT        = int(os.environ.get("MARIADB_PORT", "3306"))
DB_USER        = os.environ.get("MARIADB_USER", "root")
DB_PASS        = os.environ.get("MARIADB_PASS", "root")
DB_NAME        = os.environ.get("MARIADB_DB",   "videodisplaydb")

# Browser executables to try (in order)
BROWSER_BINS = [
    "chromium-browser", "chromium", "google-chrome",
    "google-chrome-stable",
    r"C:\Program Files\Google\Chrome\Application\chrome.exe",
    r"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
    r"C:\Program Files\Chromium\Application\chrome.exe",
]

# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# LOGGING & DB INITIALIZATION
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s  [%(levelname)-8s]  %(message)s",
    datefmt="%Y-%m-%d %H:%M:%S",
    handlers=[
        logging.StreamHandler(sys.stdout),
        logging.FileHandler(LOG_FILE),
    ],
)
log = logging.getLogger("VDP")

# Initialize MariaDB / SQLite Manager
db_mgr = None
if HAS_DB_MANAGER:
    try:
        db_mgr = HardwareDBManager(
            host=DB_HOST, port=DB_PORT, user=DB_USER, password=DB_PASS, dbname=DB_NAME
        )
    except Exception as e:
        log.warning(f"Could not initialize Database Manager: {e}")


# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# HARDWARE IDENTITY
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

def hw_mac() -> str:
    try:
        node = uuid.getnode()
        mac  = uuid.UUID(int=node).hex[-12:]
        return ":".join(mac[i:i+2].upper() for i in range(0, 12, 2))
    except Exception:
        return "00:00:00:00:00:00"

def hw_ip() -> str:
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        s.connect(("8.8.8.8", 80))
        ip = s.getsockname()[0]
        s.close()
        return ip
    except Exception:
        return "127.0.0.1"

def hw_uid() -> str:
    """Deterministic unique device ID from MAC+hostname."""
    raw = f"{hw_mac()}-{socket.gethostname()}"
    return "HW-" + hashlib.sha256(raw.encode()).hexdigest()[:12].upper()

def wifi_rssi() -> int:
    try:
        if platform.system() == "Linux":
            out = subprocess.check_output(["iwconfig"], stderr=subprocess.DEVNULL, text=True)
            m = re.search(r"Signal level=(-\d+)", out)
            if m:
                return int(m.group(1))
    except Exception:
        pass
    return -65

def sys_metrics() -> dict:
    if HAS_PSUTIL:
        cpu  = psutil.cpu_percent(interval=0.2)
        mem  = psutil.virtual_memory().percent
        try:
            disk = psutil.disk_usage('/').percent
        except Exception:
            try:
                disk = psutil.disk_usage('C:\\').percent
            except Exception:
                disk = 0.0
        uptime = int(time.time() - psutil.boot_time())
        temp = 40.0
        tf = "/sys/class/thermal/thermal_zone0/temp"
        if os.path.exists(tf):
            try:
                temp = round(int(open(tf).read().strip()) / 1000, 1)
            except Exception:
                pass
        try:
            ts = psutil.sensors_temperatures()
            for k in ("cpu_thermal","cpu-thermal","coretemp","acpitz"):
                if k in ts and ts[k]:
                    temp = round(ts[k][0].current, 1)
                    break
        except Exception:
            pass
    else:
        import random
        cpu    = round(10 + random.uniform(0, 30), 1)
        mem    = round(35 + random.uniform(0, 25), 1)
        disk   = round(50 + random.uniform(0, 10), 1)
        temp   = round(38 + random.uniform(0, 12), 1)
        uptime = 86400
    return {"cpu": cpu, "mem": mem, "disk": disk, "temp": temp, "uptime": uptime}


# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# GLOBAL SHARED STATE  (updated by MQTT commands, read by HTTP)
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
STATE = {
    "device_id":     "",
    "hw_uid":        "",
    "mac":           "",
    "ip":            "",
    "hostname":      socket.gethostname(),
    "play_state":    "IDLE",       # IDLE | PLAYING | PAUSED | STOPPED
    "current_video": None,
    "video_index":   0,
    "volume":        80,
    "brightness":    80,
    "display_on":    True,
    "elapsed_sec":   0,
    "mqtt_connected": False,
    "last_command":  None,
    # Used by HTTP server → browser to push commands
    "pending_command": None,       # {"action":"PLAY","payload":{}}
}
STATE_LOCK = threading.Lock()


# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# PLAYLIST SCANNER
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

def scan_playlist() -> list:
    MEDIA_DIR.mkdir(parents=True, exist_ok=True)
    files = []
    for f in sorted(MEDIA_DIR.iterdir()):
        if f.is_file() and f.suffix.lower() in VIDEO_EXTS:
            sz = f.stat().st_size
            files.append({
                "name":       f.name,
                "url":        f"/media/{f.name}",
                "size":       f"{round(sz / 1_048_576, 1)} MB",
                "size_bytes": sz,
                "duration":   0,   # Browser reads actual duration
            })

    # Sync to MariaDB / SQLite DB
    if db_mgr:
        try:
            db_mgr.sync_videos_to_db(files)
        except Exception:
            pass

    return files


# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# LOCAL HTTP SERVER
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

class PlayerHandler(BaseHTTPRequestHandler):
    """Handles all browser requests for the kiosk player."""

    def log_message(self, fmt, *args):
        pass  # Suppress default HTTP logs (we handle our own)

    # ── CORS + JSON helper ────────────────────────────────────
    def send_json(self, data: dict, status: int = 200):
        body = json.dumps(data).encode()
        self.send_response(status)
        self.send_header("Content-Type",   "application/json")
        self.send_header("Content-Length", str(len(body)))
        self.send_header("Access-Control-Allow-Origin",  "*")
        self.send_header("Access-Control-Allow-Methods", "GET, OPTIONS")
        self.send_header("Cache-Control",  "no-cache")
        self.end_headers()
        self.wfile.write(body)

    # ── Stream file with HTTP range support ───────────────────
    def stream_file(self, path: Path):
        if not path.exists():
            self.send_response(404)
            self.end_headers()
            return
        ct   = mimetypes.guess_type(str(path))[0] or "application/octet-stream"
        size = path.stat().st_size
        rang = self.headers.get("Range", "")

        if rang.startswith("bytes="):
            # Partial content (video seeking)
            try:
                parts = rang.replace("bytes=", "").split("-")
                start = int(parts[0]) if parts[0] else 0
                end   = int(parts[1]) if len(parts) > 1 and parts[1] else size - 1
                end   = min(end, size - 1)
                length = end - start + 1
                self.send_response(206)
                self.send_header("Content-Type",   ct)
                self.send_header("Content-Range",  f"bytes {start}-{end}/{size}")
                self.send_header("Content-Length", str(length))
                self.send_header("Accept-Ranges",  "bytes")
                self.send_header("Access-Control-Allow-Origin", "*")
                self.end_headers()
                with open(path, "rb") as f:
                    f.seek(start)
                    remaining = length
                    while remaining > 0:
                        chunk = f.read(min(65536, remaining))
                        if not chunk:
                            break
                        self.wfile.write(chunk)
                        remaining -= len(chunk)
            except Exception as e:
                log.debug(f"Range request error: {e}")
        else:
            # Full file
            self.send_response(200)
            self.send_header("Content-Type",   ct)
            self.send_header("Content-Length", str(size))
            self.send_header("Accept-Ranges",  "bytes")
            self.send_header("Access-Control-Allow-Origin", "*")
            self.end_headers()
            with open(path, "rb") as f:
                while True:
                    chunk = f.read(65536)
                    if not chunk:
                        break
                    self.wfile.write(chunk)

    def do_OPTIONS(self):
        self.send_response(200)
        self.send_header("Access-Control-Allow-Origin",  "*")
        self.send_header("Access-Control-Allow-Methods", "GET, OPTIONS")
        self.end_headers()

    def do_GET(self):
        url_path = urlparse(self.path).path.rstrip("/") or "/"

        # ── Root / player ──────────────────────────────────────
        if url_path in ("/", "/player", "/index.html", "/player.html"):
            if PLAYER_HTML.exists():
                self.stream_file(PLAYER_HTML)
            else:
                self.send_response(404)
                self.end_headers()
                self.wfile.write(b"player.html not found")

        # ── /playlist  ─────────────────────────────────────────
        elif url_path == "/playlist":
            videos = scan_playlist()
            self.send_json({"count": len(videos), "videos": videos})

        # ── /identity ──────────────────────────────────────────
        elif url_path == "/identity":
            with STATE_LOCK:
                self.send_json({
                    "deviceId": STATE["device_id"],
                    "hwUid":    STATE["hw_uid"],
                    "mac":      STATE["mac"],
                    "ip":       STATE["ip"],
                    "hostname": STATE["hostname"],
                    "platform": platform.system(),
                    "agentVersion": "3.0",
                })

        # ── /status (full playback state) ──────────────────────
        elif url_path == "/status":
            with STATE_LOCK:
                self.send_json({
                    "deviceId":     STATE["device_id"],
                    "playState":    STATE["play_state"],
                    "currentVideo": STATE["current_video"],
                    "videoIndex":   STATE["video_index"],
                    "volume":       STATE["volume"],
                    "brightness":   STATE["brightness"],
                    "elapsedSec":   STATE["elapsed_sec"],
                    "mqttConnected": STATE["mqtt_connected"],
                    "playlistCount": len(scan_playlist()),
                    "timestamp":    datetime.now(timezone.utc).isoformat(),
                })

        # ── /poll (browser polls for pending MQTT commands) ────
        elif url_path == "/poll":
            with STATE_LOCK:
                cmd = STATE.get("pending_command")
                STATE["pending_command"] = None   # consume it
            self.send_json({"command": cmd})

        # ── /media/{filename}  (stream video file) ─────────────
        elif url_path.startswith("/media/"):
            fname = Path(url_path).name
            fpath = MEDIA_DIR / fname
            if fpath.exists() and fpath.suffix.lower() in VIDEO_EXTS:
                self.stream_file(fpath)
            else:
                self.send_response(404)
                self.end_headers()

        # ── /report (browser tells us current playback state) ──
        elif url_path.startswith("/report"):
            from urllib.parse import parse_qs
            qs = parse_qs(urlparse(self.path).query)
            with STATE_LOCK:
                if "state"   in qs: STATE["play_state"]    = qs["state"][0]
                if "video"   in qs: STATE["current_video"] = qs["video"][0]
                if "index"   in qs: STATE["video_index"]   = int(qs["index"][0])
                if "elapsed" in qs: STATE["elapsed_sec"]   = int(float(qs["elapsed"][0]))
            self.send_json({"ok": True})

        # ── favicon ────────────────────────────────────────────
        elif url_path == "/favicon.ico":
            self.send_response(204)
            self.end_headers()

        else:
            self.send_response(404)
            self.send_header("Content-Type", "text/plain")
            self.end_headers()
            self.wfile.write(f"404 Not Found: {url_path}".encode())


def start_http_server():
    """Start the local HTTP server in a daemon thread."""
    server = HTTPServer(("0.0.0.0", LOCAL_PORT), PlayerHandler)
    log.info(f"🌐 Local HTTP server:  http://localhost:{LOCAL_PORT}/")
    log.info(f"   Playlist API:       http://localhost:{LOCAL_PORT}/playlist")
    log.info(f"   Identity API:       http://localhost:{LOCAL_PORT}/identity")
    log.info(f"   Media folder:       {MEDIA_DIR}")
    server.serve_forever()


# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# BROWSER LAUNCHER
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

class BrowserManager:
    def __init__(self, target_ip: str = ""):
        self._proc = None
        ip_addr = target_ip or hw_ip()
        self._url  = f"http://{ip_addr}:{LOCAL_PORT}/"

    def _find_browser(self) -> str | None:
        for b in BROWSER_BINS:
            p = Path(b)
            if p.is_file():
                return str(p)
            try:
                subprocess.check_output(
                    ["which", b], stderr=subprocess.DEVNULL, timeout=2
                )
                return b
            except Exception:
                pass
        return None

    def launch(self) -> bool:
        exe = self._find_browser()
        if not exe:
            log.warning("⚠️  Chromium/Chrome not found!")
            log.warning("   Install: sudo apt install chromium-browser")
            log.warning(f"  Manually open: {self._url}")
            return False

        flags = [
            exe,
            "--kiosk",
            "--fullscreen",
            "--no-first-run",
            "--disable-infobars",
            "--disable-pinch",
            "--overscroll-history-navigation=0",
            "--disable-session-crashed-bubble",
            "--disable-restore-session-state",
            "--disable-features=TranslateUI,Translate",
            "--autoplay-policy=no-user-gesture-required",
            "--no-sandbox",
            "--disable-dev-shm-usage",
            "--disable-web-security",
            "--allow-running-insecure-content",
            "--start-maximized",
            "--window-position=0,0",
            f"--user-data-dir=/tmp/vdp_{os.getpid()}",
            self._url,
        ]
        env = os.environ.copy()
        env["DISPLAY"] = env.get("DISPLAY", ":0")

        try:
            self._proc = subprocess.Popen(
                flags,
                stdout=subprocess.DEVNULL,
                stderr=subprocess.DEVNULL,
                env=env,
            )
            log.info(f"🌐 Browser launched (PID {self._proc.pid}) → {self._url}")
            return True
        except Exception as e:
            log.error(f"Browser launch error: {e}")
            return False

    def is_alive(self) -> bool:
        return self._proc is not None and self._proc.poll() is None

    def stop(self):
        if self._proc:
            try:
                self._proc.terminate()
            except Exception:
                pass


# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# MAIN DEVICE AGENT
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

class DeviceAgent:
    def __init__(self, device_id: str, broker: str, port: int, user: str, password: str):
        self.device_id = device_id
        self.broker    = broker
        self.port      = port
        self.running   = True
        # Populate STATE
        with STATE_LOCK:
            STATE["device_id"] = device_id
            STATE["hw_uid"]    = hw_uid()
            STATE["mac"]       = hw_mac()
            STATE["ip"]        = hw_ip()
            STATE["hostname"]  = socket.gethostname()

        self.browser = BrowserManager(target_ip=STATE["ip"])

        # MQTT client
        cid = f"VDP_HW_{device_id}_{uuid.uuid4().hex[:6]}"
        self._mqtt = mqtt.Client(client_id=cid, clean_session=True, protocol=mqtt.MQTTv5)
        self._mqtt.username_pw_set(user, password)
        self._mqtt.on_connect    = self._on_connect
        self._mqtt.on_disconnect = self._on_disconnect
        self._mqtt.on_message    = self._on_message

        # Last-Will testament
        lwt = json.dumps({
            "deviceId":  device_id,
            "hwUid":     STATE["hw_uid"],
            "mac":       STATE["mac"],
            "event":     "DEVICE_OFFLINE",
            "reason":    "UNEXPECTED",
            "timestamp": datetime.now(timezone.utc).isoformat(),
        })
        self._mqtt.will_set(f"vdp/devices/{device_id}/events", lwt, qos=1)

    # ── MQTT callbacks ────────────────────────────────────────
    def _on_connect(self, client, userdata, flags, rc, props=None):
        if rc == 0:
            with STATE_LOCK:
                STATE["mqtt_connected"] = True
            log.info(f"✅ MQTT connected → {self.broker}:{self.port}")
            client.subscribe(f"vdp/devices/{self.device_id}/commands", qos=1)
            log.info(f"📡 Subscribed: vdp/devices/{self.device_id}/commands")
            self._pub_event("DEVICE_ONLINE", {
                "mac": STATE["mac"], "ip": STATE["ip"],
                "hwUid": STATE["hw_uid"], "hostname": STATE["hostname"],
                "platform": platform.system(), "agentVersion": "3.0",
                "playlistCount": len(scan_playlist()),
            })
        else:
            log.error(f"MQTT connect failed rc={rc}")

    def _on_disconnect(self, client, userdata, rc, props=None):
        with STATE_LOCK:
            STATE["mqtt_connected"] = False
        if rc != 0:
            log.warning(f"⚠️  MQTT disconnected (rc={rc}). Reconnecting...")

    def _on_message(self, client, userdata, msg):
        try:
            payload = json.loads(msg.payload.decode())
            action  = str(payload.get("action", "")).upper()
            log.info(f"📥 MQTT command: {action}  |  {payload}")
            self._execute(action, payload)
        except Exception as e:
            log.error(f"Command error: {e}")

    # ── Command executor ──────────────────────────────────────
    def _execute(self, action: str, payload: dict):
        try:
            with STATE_LOCK:
                STATE["last_command"] = action

            if action in ("PLAY", "PAUSE", "RESUME", "STOP", "NEXT", "PREV",
                          "SET_VOLUME", "SET_BRIGHTNESS", "SYNC_MEDIA", "RELOAD"):
                # Forward command to browser via /poll endpoint
                with STATE_LOCK:
                    STATE["pending_command"] = {"action": action, "payload": payload}
                log.info(f"   → Forwarded to browser player")

            if action == "PLAY":
                with STATE_LOCK:
                    STATE["play_state"] = "PLAYING"
                self._pub_event("PLAYBACK_STARTED", payload)

            elif action == "PAUSE":
                with STATE_LOCK:
                    STATE["play_state"] = "PAUSED"
                self._pub_event("PLAYBACK_PAUSED", {})

            elif action in ("RESUME", "UNPAUSE"):
                with STATE_LOCK:
                    STATE["play_state"] = "PLAYING"
                self._pub_event("PLAYBACK_RESUMED", {})

            elif action == "STOP":
                with STATE_LOCK:
                    STATE["play_state"] = "STOPPED"
                    STATE["current_video"] = None
                self._pub_event("PLAYBACK_STOPPED", {})

            elif action == "NEXT":
                self._pub_event("PLAYBACK_NEXT", {})

            elif action == "PREV":
                self._pub_event("PLAYBACK_PREV", {})

            elif action == "SET_VOLUME":
                vol = max(0, min(100, int(payload.get("level", 80))))
                with STATE_LOCK:
                    STATE["volume"] = vol
                if platform.system() == "Linux":
                    os.system(f"amixer set Master {vol}% 2>/dev/null")
                self._pub_event("VOLUME_CHANGED", {"volume": vol})

            elif action == "SET_BRIGHTNESS":
                br = max(0, min(100, int(payload.get("level", 80))))
                with STATE_LOCK:
                    STATE["brightness"] = br
                bl = "/sys/class/backlight/rpi_backlight/brightness"
                if os.path.exists(bl):
                    open(bl, "w").write(str(int(br * 255 / 100)))
                self._pub_event("BRIGHTNESS_CHANGED", {"brightness": br})

            elif action == "SYNC_MEDIA":
                videos = scan_playlist()
                self._pub_event("MEDIA_SYNCED", {
                    "count": len(videos),
                    "files": [v["name"] for v in videos],
                })

            elif action == "GET_PLAYLIST":
                self._pub_event("PLAYLIST_INFO", {"playlist": scan_playlist()})

            elif action == "PING":
                self._pub_event("PONG", {"hostname": STATE["hostname"]})

            elif action == "REBOOT":
                log.warning("🔄 REBOOT in 5s...")
                self._pub_event("REBOOTING", {"countdown": 5})
                if platform.system() == "Linux":
                    threading.Timer(5, lambda: os.system("sudo reboot")).start()

            elif action == "SHUTDOWN":
                log.warning("⛔ SHUTDOWN in 3s...")
                self._pub_event("SHUTTING_DOWN", {})
                def _stop():
                    time.sleep(3)
                    self.running = False
                threading.Thread(target=_stop, daemon=True).start()

            elif action == "OPEN_BROWSER":
                if not self.browser.is_alive():
                    self.browser.launch()

            else:
                log.warning(f"Unknown action: {action}")
                self._pub_event("COMMAND_UNKNOWN", {"action": action})

        except Exception as e:
            log.error(f"Execute [{action}] error: {e}")
            self._pub_event("COMMAND_ERROR", {"action": action, "error": str(e)})

    # ── Event publisher ───────────────────────────────────────
    def _pub_event(self, event_type: str, data: dict):
        with STATE_LOCK:
            payload = {
                "deviceId":  STATE["device_id"],
                "hwUid":     STATE["hw_uid"],
                "mac":       STATE["mac"],
                "ip":        STATE["ip"],
                "eventType": event_type,
                "data":      data,
                "timestamp": datetime.now(timezone.utc).isoformat(),
            }
        try:
            if self._mqtt.is_connected():
                self._mqtt.publish(
                    f"vdp/devices/{self.device_id}/events",
                    json.dumps(payload), qos=0
                )
        except Exception:
            pass

    # ── Telemetry heartbeat ───────────────────────────────────
    def _heartbeat_loop(self):
        while self.running:
            try:
                m = sys_metrics()
                with STATE_LOCK:
                    telemetry = {
                        # Hardware identity
                        "deviceId":      STATE["device_id"],
                        "hwUid":         STATE["hw_uid"],
                        "macAddress":    STATE["mac"],
                        "ipAddress":     STATE["ip"],
                        "hostname":      STATE["hostname"],
                        # Metrics
                        "cpuPercent":    m["cpu"],
                        "memoryPercent": m["mem"],
                        "storagePercent": m["disk"],
                        "temperatureC":  m["temp"],
                        "uptimeSeconds": m["uptime"],
                        "networkLatencyMs": 15,
                        "signalStrength": wifi_rssi(),
                        # Playback
                        "playbackState":  {"IDLE":0,"PLAYING":1,"PAUSED":2,"STOPPED":3}.get(STATE["play_state"], 0),
                        "playbackStateStr": STATE["play_state"],
                        "currentVideo":   STATE["current_video"],
                        "videoIndex":     STATE["video_index"],
                        "playlistCount":  len(scan_playlist()),
                        "elapsedSeconds": STATE["elapsed_sec"],
                        "volume":         STATE["volume"],
                        "brightness":     STATE["brightness"],
                        "displayOn":      STATE["display_on"],
                        # Platform
                        "platform":       platform.system(),
                        "agentVersion":   "3.0",
                        "timestamp":      datetime.now(timezone.utc).isoformat(),
                    }

                if self._mqtt.is_connected():
                    self._mqtt.publish(
                        f"vdp/devices/{self.device_id}/telemetry",
                        json.dumps(telemetry), qos=0
                    )
                    log.info(
                        f"💓 Heartbeat | "
                        f"CPU:{m['cpu']:5.1f}%  "
                        f"RAM:{m['mem']:5.1f}%  "
                        f"Temp:{m['temp']:5.1f}°C  "
                        f"State:{STATE['play_state']:<8}  "
                        f"Video:{STATE['current_video'] or '-'}"
                    )
                else:
                    log.debug("MQTT not connected — skipping telemetry")

            except Exception as e:
                log.error(f"Heartbeat error: {e}")

            time.sleep(HEARTBEAT_SEC)

    # ── Browser watch (relaunch if crashes) ───────────────────
    def _browser_loop(self, enabled: bool):
        if not enabled:
            log.info("Browser auto-launch disabled (--no-browser)")
            return

        # Wait for local HTTP server to be ready
        log.info(f"⏳ Waiting for local server on port {LOCAL_PORT}...")
        for _ in range(20):
            try:
                s = socket.create_connection(("localhost", LOCAL_PORT), timeout=1)
                s.close()
                log.info("✅ Local server ready!")
                break
            except Exception:
                time.sleep(0.5)

        self.browser.launch()
        time.sleep(5)

        while self.running:
            if not self.browser.is_alive():
                log.warning("⚠️  Browser closed/crashed — relaunching in 10s...")
                time.sleep(10)
                if self.running:
                    self.browser.launch()
                    time.sleep(5)
            time.sleep(5)

    # ── Main run ──────────────────────────────────────────────
    def run(self, launch_browser: bool = True):
        with STATE_LOCK:
            uid = STATE["hw_uid"]
            mac = STATE["mac"]
            ip  = STATE["ip"]
            hn  = STATE["hostname"]

        vids = scan_playlist()

        log.info("═" * 68)
        log.info("  HBeonLabs Video Display Platform — Hardware Agent v3.0")
        log.info(f"  Device ID   : {self.device_id}")
        log.info(f"  HW UID      : {uid}")
        log.info(f"  MAC Address : {mac}")
        log.info(f"  IP Address  : {ip}")
        log.info(f"  Hostname    : {hn}")
        log.info(f"  Platform    : {platform.system()} {platform.release()}")
        log.info(f"  MQTT Broker : {self.broker}:{self.port}  (user: {MQTT_USER})")
        log.info(f"  Local Player: http://localhost:{LOCAL_PORT}/")
        log.info(f"  Media Dir   : {MEDIA_DIR}  [{len(vids)} video(s)]")
        for i, v in enumerate(vids[:10]):
            log.info(f"    [{i+1:2d}] {v['name']}")
        if len(vids) > 10:
            log.info(f"    ... and {len(vids)-10} more")
        log.info("═" * 68)

        # Graceful shutdown
        def _sig(sig, frame):
            log.info("🛑 Shutdown signal — stopping...")
            self._pub_event("DEVICE_OFFLINE", {"reason": "GRACEFUL"})
            time.sleep(0.4)
            self.running = False
            try: self._mqtt.loop_stop()
            except Exception: pass
            try: self._mqtt.disconnect()
            except Exception: pass
            self.browser.stop()
            sys.exit(0)

        signal.signal(signal.SIGINT,  _sig)
        signal.signal(signal.SIGTERM, _sig)

        # 1. Start local HTTP server
        t_http = threading.Thread(target=start_http_server, daemon=True, name="HTTP")
        t_http.start()
        time.sleep(0.3)

        # 2. Start heartbeat thread
        t_hb = threading.Thread(target=self._heartbeat_loop, daemon=True, name="Heartbeat")
        t_hb.start()

        # 3. Start browser watcher thread
        t_br = threading.Thread(target=self._browser_loop, daemon=True,
                                args=(launch_browser,), name="Browser")
        t_br.start()

        # 4. Connect MQTT
        try:
            self._mqtt.connect_async(self.broker, self.port, keepalive=60)
            self._mqtt.loop_start()
        except Exception as e:
            log.warning(f"MQTT initial connect error: {e} — will retry automatically")

        log.info("🟢 Agent running. Press Ctrl+C to stop.\n")

        while self.running:
            time.sleep(1)


# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# ENTRY POINT
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

def main():
    parser = argparse.ArgumentParser(
        description="HBeonLabs VDP Hardware Agent v3.0",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter
    )
    parser.add_argument("--device-id",  "-d",
        default=os.environ.get("VDP_DEVICE_ID", ""),
        help="Device ID (auto-generated from hardware UID if blank)"
    )
    parser.add_argument("--broker",     "-b",  default=MQTT_BROKER,  help="MQTT broker IP/host")
    parser.add_argument("--broker-port","-p",  type=int, default=MQTT_PORT, help="MQTT broker port")
    parser.add_argument("--mqtt-user",  "-u",  default=MQTT_USER,    help="MQTT username")
    parser.add_argument("--mqtt-pass",  "-P",  default=MQTT_PASS,    help="MQTT password")
    parser.add_argument("--media-dir",  "-m",
        default=str(MEDIA_DIR),
        help="Directory containing video files"
    )
    parser.add_argument("--no-browser", action="store_true",
        help="Skip browser launch (headless / server mode)"
    )
    args = parser.parse_args()

    # Update global MEDIA_DIR if custom path given
    global MEDIA_DIR
    MEDIA_DIR = Path(args.media_dir).resolve()

    # Auto-generate hardware unique device ID
    device_id = args.device_id or hw_uid()

    agent = DeviceAgent(
        device_id = device_id,
        broker    = args.broker,
        port      = args.broker_port,
        user      = args.mqtt_user,
        password  = args.mqtt_pass,
    )
    agent.run(launch_browser=not args.no_browser)


if __name__ == "__main__":
    main()
