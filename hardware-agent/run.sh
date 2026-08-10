#!/bin/bash
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# HBeonLabs Video Display Platform — All-In-One Linux Installer & Launcher
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

set -e

# Change directory to script location
CDIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$CDIR"

echo "================================================================="
echo "  HBeonLabs Video Display Platform — Hardware Agent Setup & Run  "
echo "================================================================="

# 1. Detect System Architecture & IP Address
DEVICE_IP=$(hostname -I 2>/dev/null | awk '{print $1}')
if [ -z "$DEVICE_IP" ]; then
    DEVICE_IP="127.0.0.1"
fi
HOSTNAME=$(hostname)

echo "📌 Device Hostname : $HOSTNAME"
echo "📌 Device Local IP : $DEVICE_IP"
echo "📌 Target Player   : http://$DEVICE_IP:8765/"
echo "-----------------------------------------------------------------"

# 2. Check & Install System Packages (Python3, Pip, Chromium, MariaDB)
echo "🔍 Checking system dependencies..."

if ! command -v python3 &> /dev/null || ! command -v pip3 &> /dev/null; then
    echo "📦 Installing Python3 and Pip..."
    sudo apt-get update && sudo apt-get install -y python3 python3-pip python3-venv
fi

if ! command -v chromium-browser &> /dev/null && ! command -v google-chrome &> /dev/null && ! command -v chromium &> /dev/null; then
    echo "📦 Installing Chromium Browser for Kiosk display..."
    sudo apt-get update && sudo apt-get install -y chromium-browser || sudo apt-get install -y chromium
fi

if ! command -v mariadb &> /dev/null && ! command -v mysql &> /dev/null; then
    echo "📦 Installing MariaDB Server..."
    sudo apt-get update && sudo apt-get install -y mariadb-server mariadb-client || true
    sudo systemctl start mariadb 2>/dev/null || true
    sudo systemctl enable mariadb 2>/dev/null || true
fi

# 3. Install Python Dependencies
echo "📦 Installing Python packages from requirements.txt..."
python3 -m pip install -r requirements.txt --quiet --break-system-packages 2>/dev/null || \
python3 -m pip install -r requirements.txt --quiet || true

# 4. Import MariaDB SQL Schema if MariaDB is running
if command -v mariadb &> /dev/null || command -v mysql &> /dev/null; then
    if [ -f "schema.sql" ]; then
        echo "🗄️ Importing schema.sql into MariaDB database (videodisplaydb)..."
        sudo mysql < schema.sql 2>/dev/null || mysql -u root -proot < schema.sql 2>/dev/null || true
    fi
fi

# 5. Create media/videos directory
mkdir -p media/videos

# 6. Check if videos exist; if not, create a notice
VIDEO_COUNT=$(find media/videos -type f \( -name "*.mp4" -o -name "*.avi" -o -name "*.mkv" -o -name "*.mov" -o -name "*.webm" \) 2>/dev/null | wc -l)
echo "🎬 Found $VIDEO_COUNT video file(s) in media/videos/"

echo "--------------------------------================-----------------"
echo "🚀 Launching Hardware Agent..."
echo "🌐 Kiosk player running at: http://$DEVICE_IP:8765/"
echo "================================================================="

# Run the Python Hardware Agent
python3 device_agent.py "$@"
