#!/usr/bin/env python3
"""
HBeonLabs VDP — Hardware Installer
===================================
Run this ONCE to set up the hardware device:
  1. Installs Python dependencies
  2. Creates the media/videos folder
  3. Copies any .mp4/.avi/.mkv files you drop next to this script into media/videos/
  4. Writes a .env config file with your settings
  5. Creates a startup script (run.sh / run.bat)

Usage:
  python installer.py                          # interactive setup
  python installer.py --server 66.116.227.217  # set server IP directly
  python installer.py --device-id 1            # set device ID directly
"""

import argparse
import hashlib
import os
import platform
import shutil
import socket
import subprocess
import sys
import uuid
from pathlib import Path

BASE_DIR  = Path(__file__).parent
MEDIA_DIR = BASE_DIR / "media" / "videos"

VIDEO_EXTS = {".mp4", ".avi", ".mkv", ".mov", ".webm", ".ts", ".m4v", ".flv"}

BANNER = """
╔══════════════════════════════════════════════════════════╗
║     HBeonLabs Video Display Platform — Installer        ║
║     Hardware Agent v2.0                                  ║
╚══════════════════════════════════════════════════════════╝
"""

def get_mac():
    mac = uuid.UUID(int=uuid.getnode()).hex[-12:]
    return ":".join(mac[i:i+2].upper() for i in range(0, 12, 2))

def get_ip():
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        s.connect(("8.8.8.8", 80))
        ip = s.getsockname()[0]
        s.close()
        return ip
    except Exception:
        return "127.0.0.1"

def get_hw_uid():
    raw = f"{get_mac()}-{socket.gethostname()}"
    return "HW-" + hashlib.sha256(raw.encode()).hexdigest()[:12].upper()

def install_deps():
    print("\n📦 Installing Python dependencies...")
    req = BASE_DIR / "requirements.txt"
    result = subprocess.run(
        [sys.executable, "-m", "pip", "install", "-r", str(req)],
        capture_output=False
    )
    if result.returncode == 0:
        print("   ✅ Dependencies installed.")
    else:
        print("   ⚠️  Some dependencies failed. Check pip output above.")

def create_media_dir():
    MEDIA_DIR.mkdir(parents=True, exist_ok=True)
    print(f"\n📁 Media folder ready: {MEDIA_DIR}")

def import_local_videos():
    """Copy any video files dropped next to installer.py into media/videos/."""
    imported = 0
    for f in BASE_DIR.iterdir():
        if f.is_file() and f.suffix.lower() in VIDEO_EXTS and f.parent == BASE_DIR:
            dest = MEDIA_DIR / f.name
            if not dest.exists():
                shutil.copy2(f, dest)
                print(f"   📥 Imported: {f.name}")
                imported += 1
    if imported == 0:
        print(f"   ℹ️  No video files found next to installer.py.")
        print(f"   Drop .mp4/.avi/.mkv files into: {MEDIA_DIR}")
    else:
        print(f"   ✅ Imported {imported} video file(s).")
    return imported

def write_env(config: dict):
    env_path = BASE_DIR / ".env"
    lines = [
        "# HBeonLabs VDP Hardware Agent Configuration",
        f"VDP_DEVICE_ID={config['device_id']}",
        f"VDP_MQTT_BROKER={config['mqtt_broker']}",
        f"VDP_MQTT_PORT={config['mqtt_port']}",
        f"VDP_MQTT_USER={config['mqtt_user']}",
        f"VDP_MQTT_PASS={config['mqtt_pass']}",
        f"VDP_SERVER_IP={config['server_ip']}",
        f"VDP_SERVER_PORT={config['server_port']}",
        f"VDP_MEDIA_DIR={MEDIA_DIR}",
    ]
    env_path.write_text("\n".join(lines) + "\n")
    print(f"\n⚙️  Config saved to: {env_path}")

def write_startup_script(config: dict):
    if platform.system() == "Windows":
        bat = BASE_DIR / "run.bat"
        content = f"""@echo off
title HBeonLabs VDP Hardware Agent
echo Starting VDP Hardware Agent...
cd /d "{BASE_DIR}"
python device_agent.py --device-id {config['device_id']} --broker {config['mqtt_broker']} --broker-port {config['mqtt_port']} --mqtt-user {config['mqtt_user']} --mqtt-pass {config['mqtt_pass']} --server-ip {config['server_ip']} --server-port {config['server_port']}
pause
"""
        bat.write_text(content)
        print(f"\n🚀 Startup script (Windows): {bat}")
        print(f"   Double-click run.bat to start the agent.")
    else:
        sh = BASE_DIR / "run.sh"
        content = f"""#!/bin/bash
# HBeonLabs VDP Hardware Agent Startup Script
cd "$(dirname "$0")"
echo "Starting VDP Hardware Agent..."
python3 device_agent.py \\
    --device-id {config['device_id']} \\
    --broker {config['mqtt_broker']} \\
    --broker-port {config['mqtt_port']} \\
    --mqtt-user {config['mqtt_user']} \\
    --mqtt-pass {config['mqtt_pass']} \\
    --server-ip {config['server_ip']} \\
    --server-port {config['server_port']}
"""
        sh.write_text(content)
        sh.chmod(0o755)
        print(f"\n🚀 Startup script (Linux): {sh}")
        print(f"   Run: ./run.sh")

def print_summary(config: dict, video_count: int):
    mac = get_mac()
    ip  = get_ip()
    uid = get_hw_uid()
    print(f"""
╔══════════════════════════════════════════════════════════╗
║                 Installation Complete ✅                 ║
╠══════════════════════════════════════════════════════════╣
║  Hardware Identity:                                      ║
║    Device ID : {config['device_id']:<40} ║
║    HW UID    : {uid:<40} ║
║    MAC Addr  : {mac:<40} ║
║    IP Address: {ip:<40} ║
║    Hostname  : {socket.gethostname():<40} ║
╠══════════════════════════════════════════════════════════╣
║  MQTT Broker : {config['mqtt_broker']+':'+str(config['mqtt_port']):<40} ║
║  MQTT User   : {config['mqtt_user']:<40} ║
║  Web Server  : {config['server_ip']+':'+str(config['server_port']):<40} ║
║  Videos found: {str(video_count):<40} ║
╠══════════════════════════════════════════════════════════╣
║  Browser opens: http://localhost:8765/                   ║
║  Login: admin / admin123                                 ║
╚══════════════════════════════════════════════════════════╝

Next steps:
  1. Add videos to: {MEDIA_DIR}
  2. Run:  {'run.bat' if platform.system()=='Windows' else './run.sh'}

The browser will auto-open and play videos in sequence! 🎬
""")

def main():
    print(BANNER)

    parser = argparse.ArgumentParser(description="HBeonLabs VDP Hardware Installer")
    parser.add_argument("--device-id",  default="",                  help="Device ID (auto-generated if blank)")
    parser.add_argument("--server",     default="66.116.227.217",    help="Web server / MQTT broker IP")
    parser.add_argument("--server-port",type=int, default=5000,      help="Web server port")
    parser.add_argument("--mqtt-port",  type=int, default=1883,      help="MQTT broker port")
    parser.add_argument("--mqtt-user",  default="admin",             help="MQTT username")
    parser.add_argument("--mqtt-pass",  default="admin123",          help="MQTT password")
    parser.add_argument("--skip-deps",  action="store_true",         help="Skip pip install")
    args = parser.parse_args()

    # Auto-generate device ID from hardware
    device_id = args.device_id or get_hw_uid()

    config = {
        "device_id":   device_id,
        "mqtt_broker": args.server,
        "mqtt_port":   args.mqtt_port,
        "mqtt_user":   args.mqtt_user,
        "mqtt_pass":   args.mqtt_pass,
        "server_ip":   args.server,
        "server_port": args.server_port,
    }

    print(f"Device ID  : {device_id}")
    print(f"HW UID     : {get_hw_uid()}")
    print(f"MAC Address: {get_mac()}")
    print(f"IP Address : {get_ip()}")
    print(f"Hostname   : {socket.gethostname()}")
    print(f"MQTT Broker: {args.server}:{args.mqtt_port}  (user: {args.mqtt_user})")

    if not args.skip_deps:
        install_deps()

    create_media_dir()
    video_count = import_local_videos()
    write_env(config)
    write_startup_script(config)
    print_summary(config, video_count)


if __name__ == "__main__":
    main()
