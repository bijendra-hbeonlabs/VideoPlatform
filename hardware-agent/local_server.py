#!/usr/bin/env python3
"""
HBeonLabs VDP — Local Hardware HTTP Server
==========================================
A tiny HTTP server that:
  - Serves player.html at http://localhost:8765/
  - Serves video files from media/videos/ folder
  - Exposes /playlist API  →  list of videos for the browser player
  - Exposes /identity API  →  hardware MAC, IP, hostname, device-ID
  - Exposes /status  API  →  current playback state

Started automatically by device_agent.py.
Can also be run standalone:  python local_server.py
"""

import json
import mimetypes
import os
import socket
import hashlib
import platform
import uuid
import time
from pathlib import Path
from http.server import BaseHTTPRequestHandler, HTTPServer
from urllib.parse import urlparse, parse_qs

# ── Config ────────────────────────────────────────────────────
BASE_DIR   = Path(__file__).parent
MEDIA_DIR  = BASE_DIR / "media" / "videos"
PLAYER_HTML = BASE_DIR / "player.html"
PORT       = 8765
VIDEO_EXTS = {".mp4", ".avi", ".mkv", ".mov", ".webm", ".ts", ".m4v", ".flv"}

# ── Shared state (set by device_agent) ───────────────────────
shared_state = {
    "device_id":   "1",
    "hw_uid":      "",
    "mac":         "",
    "ip":          "",
    "hostname":    socket.gethostname(),
    "play_state":  "IDLE",
    "current_video": None,
    "volume":      80,
    "brightness":  80,
    "elapsed":     0,
    "playlist":    [],
}

def get_mac_address() -> str:
    try:
        mac = uuid.UUID(int=uuid.getnode()).hex[-12:]
        return ":".join(mac[i:i+2].upper() for i in range(0, 12, 2))
    except Exception:
        return "00:00:00:00:00:00"

def get_ip_address() -> str:
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        s.connect(("8.8.8.8", 80))
        ip = s.getsockname()[0]
        s.close()
        return ip
    except Exception:
        return socket.gethostbyname(socket.gethostname())

def get_hardware_uid() -> str:
    mac = get_mac_address()
    raw = f"{mac}-{socket.gethostname()}"
    return "HW-" + hashlib.sha256(raw.encode()).hexdigest()[:12].upper()

def scan_playlist() -> list[dict]:
    """Scan media folder and return playlist metadata."""
    MEDIA_DIR.mkdir(parents=True, exist_ok=True)
    videos = []
    for f in sorted(MEDIA_DIR.iterdir()):
        if f.is_file() and f.suffix.lower() in VIDEO_EXTS:
            videos.append({
                "name":     f.name,
                "url":      f"/media/videos/{f.name}",
                "size":     f"{round(f.stat().st_size / 1_048_576, 1)} MB",
                "size_bytes": f.stat().st_size,
                "duration": 0,  # Browser will read actual duration via video element
            })
    return videos


class VDPHandler(BaseHTTPRequestHandler):
    """Minimal HTTP handler for the hardware kiosk server."""

    def log_message(self, format, *args):
        pass  # suppress default access logs

    def send_json(self, data: dict, status: int = 200):
        body = json.dumps(data, indent=2).encode("utf-8")
        self.send_response(status)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(body)))
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Cache-Control", "no-cache")
        self.end_headers()
        self.wfile.write(body)

    def send_file(self, path: Path, content_type: str | None = None):
        if not path.exists():
            self.send_response(404)
            self.end_headers()
            return

        ct = content_type or mimetypes.guess_type(str(path))[0] or "application/octet-stream"
        size = path.stat().st_size
        rang = self.headers.get("Range")

        if rang and rang.startswith("bytes="):
            # Support HTTP range requests for video seeking
            parts = rang.replace("bytes=", "").split("-")
            start = int(parts[0]) if parts[0] else 0
            end   = int(parts[1]) if len(parts) > 1 and parts[1] else size - 1
            end   = min(end, size - 1)
            length = end - start + 1

            self.send_response(206)
            self.send_header("Content-Type", ct)
            self.send_header("Content-Range", f"bytes {start}-{end}/{size}")
            self.send_header("Content-Length", str(length))
            self.send_header("Accept-Ranges", "bytes")
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
        else:
            self.send_response(200)
            self.send_header("Content-Type", ct)
            self.send_header("Content-Length", str(size))
            self.send_header("Accept-Ranges", "bytes")
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
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Methods", "GET, OPTIONS")
        self.end_headers()

    def do_GET(self):
        parsed = urlparse(self.path)
        path   = parsed.path.rstrip("/") or "/"

        # ── Routes ────────────────────────────────────────────
        if path in ("/", "/player", "/index.html"):
            self.send_file(PLAYER_HTML, "text/html")

        elif path == "/identity":
            mac = get_mac_address()
            self.send_json({
                "deviceId": shared_state.get("device_id", "1"),
                "hwUid":    get_hardware_uid(),
                "mac":      mac,
                "ip":       get_ip_address(),
                "hostname": socket.gethostname(),
                "platform": platform.system(),
            })

        elif path == "/playlist":
            videos = scan_playlist()
            self.send_json({
                "count":  len(videos),
                "videos": videos,
            })

        elif path == "/status":
            self.send_json({
                "deviceId":     shared_state.get("device_id"),
                "playState":    shared_state.get("play_state"),
                "currentVideo": shared_state.get("current_video"),
                "volume":       shared_state.get("volume"),
                "brightness":   shared_state.get("brightness"),
                "elapsed":      shared_state.get("elapsed"),
                "playlistCount": len(scan_playlist()),
                "timestampUtc": time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime()),
            })

        elif path.startswith("/media/videos/"):
            filename = Path(path).name
            file_path = MEDIA_DIR / filename
            if file_path.exists() and file_path.suffix.lower() in VIDEO_EXTS:
                self.send_file(file_path)
            else:
                self.send_response(404)
                self.end_headers()

        elif path == "/favicon.ico":
            self.send_response(204)
            self.end_headers()

        else:
            self.send_response(404)
            self.send_header("Content-Type", "text/plain")
            self.end_headers()
            self.wfile.write(b"404 Not Found")


def run_server(port: int = PORT):
    """Start the local HTTP server. Blocks forever."""
    server = HTTPServer(("0.0.0.0", port), VDPHandler)
    print(f"✅ VDP Local Server running at http://localhost:{port}")
    print(f"   Player URL : http://localhost:{port}/")
    print(f"   Playlist   : http://localhost:{port}/playlist")
    print(f"   Identity   : http://localhost:{port}/identity")
    print(f"   Videos dir : {MEDIA_DIR}")
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        print("\nServer stopped.")
        server.server_close()


if __name__ == "__main__":
    run_server()
