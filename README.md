# 🎬 HBeonLabs Video Display Platform (VDP)

An enterprise-grade, end-to-end Digital Signage & Video Display Platform designed for remote fleet management, automated broadcast scheduling, and real-time hardware display playback.

---

## 🌟 Key Features

- 🖥️ **Web Management Dashboard**: Modern dark-themed glassmorphism UI built with **C# .NET 9 & Blazor Interactive Server**.
- 🔐 **Secure Authentication**: Built-in Admin authentication (`admin` / `admin123`).
- 📡 **Real-time MQTT Communication**: Sub-second telemetry reporting and remote command execution via Mosquitto MQTT Broker (`66.116.227.217:1883`).
- 🎬 **Full-screen Kiosk Player (`player.html`)**: Self-contained HTML5 video player with sequential playback, loop/replay, time elapsed indicators, and light theme.
- 🐍 **Hardware Agent (`device_agent.py` v3.0)**: Production Python agent for **Raspberry Pi, NVIDIA Jetson, Intel NUC, and Ubuntu Linux** hardware displays.
- 🗄️ **Database Persistence**: Complete **MariaDB / MySQL** integration (`schema.sql`) with automatic local SQLite fallback (`hardware_local.db`).
- ⚡ **1-Click Hardware Setup (`run.sh`)**: Auto-installs system packages, configures database, detects hardware IP, and launches fullscreen Kiosk browser.

---

## 🏗️ System Architecture

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                    VIDEO DISPLAY PLATFORM SERVER                                │
│              (.NET 9 Blazor Enterprise Web Dashboard)                           │
│                      http://66.116.227.217:5000                                 │
│                                                                                 │
│  ┌─────────────────────────┐         ┌──────────────────────────────────────┐  │
│  │  Blazor Web Dashboard   │◄───────►│  MQTT Broker (Mosquitto)             │  │
│  │  ├─ Devices Fleet       │         │  Host: 66.116.227.217:1883           │  │
│  │  ├─ Playlists & Media   │         │  User: admin / Pass: admin123        │  │
│  │  ├─ Broadcast Schedules │         │                                      │  │
│  │  └─ Command Dispatcher  │         │                                      │  │
│  └─────────────────────────┘         └──────────────────────────────────────┘  │
└──────────────────────────────────────────────────┬──────────────────────────────┘
                                                   │
                                  MQTT TCP (vdp/devices/+)
                                                   │
      ┌────────────────────────────────────────────┴────────────────────────────────────────────┐
      │                                                                                         │
┌─────▼──────────────────────────────────────────────────┐   ┌──────────────────────────────────▼───────────────────┐
│  HARDWARE DEVICE 1 (Ubuntu Linux / Raspberry Pi)       │   │  HARDWARE DEVICE 2 (NVIDIA Jetson / Intel NUC)        │
│  IP Access: http://192.168.1.50:8765/                  │   │  IP Access: http://192.168.1.51:8765/                  │
│                                                        │   │                                                       │
│  ┌──────────────────────────────────────────────────┐  │   │  ┌──────────────────────────────────────────────────┐  │
│  │ device_agent.py (Python Hardware Agent v3.0)     │  │   │  │ device_agent.py (Python Hardware Agent v3.0)     │  │
│  │ ├─ Local HTTP Server (Port 8765)                 │  │   │  │ ├─ Local HTTP Server (Port 8765)                 │  │
│  │ ├─ MariaDB / MySQL DB (videodisplaydb)           │  │   │  │ ├─ MariaDB / MySQL DB (videodisplaydb)           │  │
│  │ ├─ MQTT Client (Pub Telemetry / Sub Commands)    │  │   │  │ ├─ MQTT Client (Pub Telemetry / Sub Commands)    │  │
│  │ └─ Chromium Kiosk Browser (Full-screen Player)   │  │   │  │ └─ Chromium Kiosk Browser (Full-screen Player)   │  │
│  └──────────────────────────────────────────────────┘  │   │  └──────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────┘   └───────────────────────────────────────────────────────┘
```

---

## 📁 Repository Structure

```
VideoPlatform/
├── src/
│   ├── VideoDisplayPlatform.Domain/          ← Domain Entity Models
│   ├── VideoDisplayPlatform.Application/     ← Core State & Interfaces
│   │   └── Services/
│   │       ├── AuthService.cs                ← Admin login & hardware identity
│   │       └── DevicePlatformStateService.cs ← In-memory state & telemetry sync
│   ├── VideoDisplayPlatform.Infrastructure/  ← EF Core DbContext & MQTT Service
│   │   ├── Persistence/AppDbContext.cs       ← MariaDB / MySQL DbContext & Migrations
│   │   └── Messaging/MqttService.cs          ← MQTTnet v5 client integration
│   ├── VideoDisplayPlatform.DeviceAgent/     ← .NET Device Agent Background Worker
│   └── VideoDisplayPlatform.Web/             ← Blazor Interactive Web Application
│       ├── appsettings.json                  ← Server & MQTT config
│       ├── Components/Pages/
│       │   ├── Login.razor                   ← Admin Sign-in page
│       │   ├── Home.razor                    ← KPI & Fleet Dashboard
│       │   ├── Devices.razor                 ← Add & Manage Devices Form
│       │   ├── Playlists.razor               ← Media library & playlist builder
│       │   ├── Schedules.razor               ← Broadcast schedule matrix
│       │   ├── Commands.razor                ← Live MQTT Command Dispatcher
│       │   └── Alerts.razor                  ← Health incident manager
└── hardware-agent/                           ← Python Hardware Agent Package
    ├── device_agent.py                       ← Main Python Hardware Agent (v3.0)
    ├── player.html                           ← Fullscreen Browser Kiosk Player
    ├── db_manager.py                         ← MariaDB / MySQL & SQLite DB Manager
    ├── schema.sql                            ← Enterprise MariaDB SQL Schema
    ├── .env                                  ← Hardware configuration
    ├── run.sh                                ← 1-Click Linux Setup & Launch Script
    ├── vdp-agent.service                     ← Linux systemd service for boot auto-start
    └── requirements.txt                      ← Python dependencies
```

---

## 🚀 Quick Start Guide

### 1️⃣ Run the Web Dashboard (.NET Web Server)

```powershell
cd src/VideoDisplayPlatform.Web
dotnet run
```

- **Dashboard URL**: `http://localhost:5000` (or `http://66.116.227.217:5000`)
- **Login Credentials**:
  - **Username**: `admin`
  - **Password**: `admin123`

---

### 2️⃣ Run the Hardware Agent on Ubuntu Display Hardware

Copy the `hardware-agent` folder to your display hardware (Raspberry Pi, NVIDIA Jetson, NUC, Ubuntu PC) and run:

```bash
cd hardware-agent
chmod +x run.sh
./run.sh
```

#### What `./run.sh` automatically does:
1. Detects your hardware's **Local IP Address** (e.g., `http://192.168.1.50:8765/`).
2. Installs `python3`, `pip`, `chromium-browser`, and `mariadb-server` automatically if missing.
3. Installs Python dependencies (`paho-mqtt`, `psutil`, `pymysql`, `python-dotenv`).
4. Executes `schema.sql` to initialize the MariaDB database (`videodisplaydb`).
5. Launches the **Local HTTP Video Server** on port `8765`.
6. Launches **Chromium Browser in Kiosk Mode** at `http://<DEVICE_IP>:8765/`.
7. Connects to the central **MQTT Broker (`66.116.227.217:1883`)**.

---

## 📡 MQTT Topics & Remote Controls

### Broker Configuration (`appsettings.json` & `.env`)
- **Host**: `66.116.227.217`
- **Port**: `1883`
- **Username**: `admin`
- **Password**: `admin123`

### MQTT Topics
| Topic | Direction | Content |
|-------|-----------|---------|
| `vdp/devices/{id}/telemetry` | Hardware → Web | CPU, RAM, Temp, Uptime, Video status every 5s |
| `vdp/devices/{id}/commands` | Web → Hardware | PLAY, PAUSE, STOP, NEXT, SET_VOLUME, REBOOT commands |
| `vdp/devices/{id}/events` | Hardware → Web | Status events (ONLINE, PLAYBACK_STARTED, ERROR) |
| `vdp/devices/register` | Hardware → Web | Device auto-registration (MAC, IP, Hostname) |

### Remote Commands Supported
| Command | Payload Example | Action |
|---------|-----------------|--------|
| `PLAY` | `{"videoPath": "ad.mp4"}` | Play video |
| `PAUSE` | `{}` | Pause video |
| `RESUME` | `{}` | Resume video |
| `STOP` | `{}` | Stop video |
| `NEXT` | `{}` | Skip to next video |
| `PREV` | `{}` | Skip to previous video |
| `SET_VOLUME` | `{"level": 85}` | Set volume (0-100) |
| `SET_BRIGHTNESS` | `{"level": 90}` | Set screen brightness (0-100) |
| `SYNC_MEDIA` | `{}` | Re-scan video folder & sync DB |
| `REBOOT` | `{}` | Reboot Ubuntu device |

---

## 🎬 Adding Videos to Display Hardware

Simply copy `.mp4`, `.avi`, `.mkv`, `.mov`, or `.webm` files into:
```
hardware-agent/media/videos/
```
The hardware agent will automatically scan them, store them in MariaDB, and play them in sequence with loop/replay!

---

## 🧪 Verification & Build Commands

### Build Solution
```powershell
dotnet build
```

### Run Unit Tests
```powershell
dotnet test
```

### Validate Hardware Agent Python Syntax
```bash
python3 -c "import ast; ast.parse(open('hardware-agent/device_agent.py').read()); print('Syntax OK')"
```

---

## ⚙️ Auto-Start on Boot (Linux systemd)

To make the hardware agent start automatically when the Ubuntu device powers ON:

```bash
sudo cp hardware-agent/vdp-agent.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl enable vdp-agent
sudo systemctl start vdp-agent
```

Check live status:
```bash
sudo journalctl -u vdp-agent -f
```

---

## 📄 License & Copyright

© 2026 HBeonLabs Systems. All rights reserved.
