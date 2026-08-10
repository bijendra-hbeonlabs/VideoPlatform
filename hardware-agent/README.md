# HBeonLabs Video Display Platform — Enterprise Hardware Agent v3.0

Complete enterprise hardware agent package for **Linux (Ubuntu / Raspberry Pi OS / NVIDIA Jetson / Intel NUC)** and **Windows IoT**.

---

## 🏗️ Architecture — How Hardware Connects with .NET Web Platform

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    VIDEO DISPLAY PLATFORM SERVER                        │
│                 (.NET 9 Blazor Enterprise Dashboard)                    │
│                        http://66.116.227.217:5000                       │
│                                                                         │
│  ┌────────────────────────┐         ┌────────────────────────────────┐  │
│  │ Blazor Web Dashboard   │◄───────►│  MQTT Broker (Mosquitto)       │  │
│  │ (Devices, Playlists,   │         │  Host: 66.116.227.217:1883     │  │
│  │  Schedules, Commands)  │         │  User: admin / Pass: admin123  │  │
│  └────────────────────────┘         └────────────────────────────────┘  │
└────────────────────────────────────────────────┬────────────────────────┘
                                                 │
                                MQTT TCP (vdp/devices/+)
                                                 │
      ┌──────────────────────────────────────────┴──────────────────────────────────────────┐
      │                                                                                     │
┌─────▼─────────────────────────────────────────────────┐   ┌───────────────────────────────▼───────────────────────┐
│  HARDWARE DEVICE 1 (Ubuntu Linux / Raspberry Pi)      │   │  HARDWARE DEVICE 2 (NVIDIA Jetson / Intel NUC)        │
│  IP: http://192.168.1.50:8765/                        │   │  IP: http://192.168.1.51:8765/                        │
│                                                       │   │                                                       │
│  ┌─────────────────────────────────────────────────┐  │   │  ┌─────────────────────────────────────────────────┐  │
│  │ device_agent.py (Python Agent v3.0)             │  │   │  │ device_agent.py (Python Agent v3.0)             │  │
│  │ ├─ Local HTTP Server (Port 8765)                │  │   │  │ ├─ Local HTTP Server (Port 8765)                │  │
│  │ ├─ MariaDB / MySQL DB (videodisplaydb)          │  │   │  │ ├─ MariaDB / MySQL DB (videodisplaydb)          │  │
│  │ ├─ MQTT Client (Pub Telemetry / Sub Commands)   │  │   │  │ ├─ MQTT Client (Pub Telemetry / Sub Commands)   │  │
│  │ └─ Chromium Kiosk Browser (Full-screen Player)  │  │   │  │ └─ Chromium Kiosk Browser (Full-screen Player)  │  │
│  └─────────────────────────────────────────────────┘  │   │  └─────────────────────────────────────────────────┘  │
└───────────────────────────────────────────────────────┘   └───────────────────────────────────────────────────────┘
```

---

## ⚡ 1-Click Installation & Setup on Ubuntu Hardware

Simply copy the `hardware-agent` folder to your Ubuntu device and run:

```bash
cd hardware-agent
chmod +x run.sh
./run.sh
```

### What `./run.sh` automatically does:
1. Detects your hardware's **Local IP Address** (e.g., `192.168.1.50`).
2. Installs `python3`, `pip`, `chromium-browser`, and `mariadb-server` automatically if not installed.
3. Installs Python dependencies (`paho-mqtt`, `psutil`, `pymysql`, `python-dotenv`).
4. Executes `schema.sql` to initialize MariaDB database (`videodisplaydb`).
5. Launches the **Local HTTP Video Server** on port `8765`.
6. Launches **Chromium Browser in Kiosk Mode** at `http://<DEVICE_IP>:8765/`.
7. Connects to the central **MQTT Broker (`66.116.227.217:1883`)**.

---

## 🌐 Remote IP Management

Once `device_agent.py` is running, you can manage and view the display from any browser in your local network or server:

- **Display Kiosk Player**: `http://<DEVICE_IP>:8765/`
- **Playlist REST API**: `http://<DEVICE_IP>:8765/playlist`
- **Device Identity API**: `http://<DEVICE_IP>:8765/identity`
- **Device Health Status**: `http://<DEVICE_IP>:8765/status`

---

## 🗄️ MariaDB / MySQL Database Integration

The agent includes enterprise database logging via `db_manager.py` and `schema.sql`.

### Database Credentials (`.env` file):
```ini
MARIADB_HOST=localhost
MARIADB_PORT=3306
MARIADB_USER=root
MARIADB_PASS=root
MARIADB_DB=videodisplaydb
```

### Database Tables:
1. `hardware_devices` — Device registration, MAC address, IP, status.
2. `hardware_videos` — Video media catalog, file paths, size, play order.
3. `hardware_playback_logs` — Live playback events (PLAY, PAUSE, ENDED).
4. `hardware_telemetry_logs` — Historical hardware health (CPU, RAM, Temp, Uptime).
5. `hardware_settings` — Configuration key-value settings.

*Note: If MariaDB is not running, the agent automatically falls back to a local SQLite database (`hardware_local.db`), ensuring zero downtime!*

---

## 📡 Connecting with the .NET Blazor Web Server

### 1. Start the .NET Web Server:
```powershell
cd src/VideoDisplayPlatform.Web
dotnet run
```
Web Dashboard will run at: `http://localhost:5000` (or `http://66.116.227.217:5000`).

### 2. Login Credentials:
- **Username**: `admin`
- **Password**: `admin123`

### 3. Remote Control Commands:
From the Web Dashboard (`/commands` or `/devices`), you can send real-time MQTT commands to any hardware device:

| Action | Payload Example | Description |
|--------|-----------------|-------------|
| `PLAY` | `{"videoPath": "ad1.mp4"}` | Start playing specific video |
| `PAUSE` | `{}` | Pause video playback |
| `RESUME` | `{}` | Resume paused video |
| `STOP` | `{}` | Stop video playback |
| `NEXT` | `{}` | Skip to next video in playlist |
| `PREV` | `{}` | Go to previous video |
| `SET_VOLUME` | `{"level": 85}` | Adjust hardware audio volume |
| `SET_BRIGHTNESS` | `{"level": 90}` | Adjust display backlight brightness |
| `SYNC_MEDIA` | `{}` | Re-scan media folder and update database |
| `REBOOT` | `{}` | Safely reboot Ubuntu device |

---

## 🚀 Auto-Start on System Boot (Linux systemd)

To make the hardware agent auto-start whenever the Ubuntu device powers ON:

```bash
sudo cp vdp-agent.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl enable vdp-agent
sudo systemctl start vdp-agent
```

Check service status and live logs:
```bash
sudo systemctl status vdp-agent
sudo journalctl -u vdp-agent -f
```

---

## 🎬 Adding Videos

Simply drop your video files (`.mp4`, `.avi`, `.mkv`, `.webm`, `.mov`) into:
```
hardware-agent/media/videos/
```
The agent automatically detects new files, syncs them to MariaDB, and includes them in the loop playback!
