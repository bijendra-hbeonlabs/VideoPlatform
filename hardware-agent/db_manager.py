#!/usr/bin/env python3
"""
HBeonLabs VDP — Hardware Database Manager (MariaDB / MySQL)
===========================================================
Handles storing video playlists, playback history logs, and device settings
into MariaDB/MySQL database with automatic SQLite fallback if MariaDB is unavailable.
"""

import logging
import os
import sqlite3
import time
from datetime import datetime, timezone
from pathlib import Path

# Try importing MariaDB / MySQL connectors
HAS_MYSQL = False
try:
    import pymysql
    HAS_MYSQL = True
    MYSQL_DRIVER = "pymysql"
except ImportError:
    try:
        import mysql.connector as pymysql
        HAS_MYSQL = True
        MYSQL_DRIVER = "mysql.connector"
    except ImportError:
        HAS_MYSQL = False

log = logging.getLogger("VDP-DB")


class HardwareDBManager:
    """Manages MariaDB / MySQL or SQLite database for the hardware agent."""

    def __init__(self, host="localhost", port=3306, user="root", password="root", dbname="videodisplaydb"):
        self.host = host
        self.port = int(port)
        self.user = user
        self.password = password
        self.dbname = dbname
        self.use_sqlite = False

        # SQLite fallback file in case MariaDB is unreachable
        self.sqlite_path = Path(__file__).parent / "hardware_local.db"
        self.init_db()

    def get_connection(self):
        """Get database connection (MariaDB or SQLite fallback)."""
        if not self.use_sqlite and HAS_MYSQL:
            try:
                if MYSQL_DRIVER == "pymysql":
                    return pymysql.connect(
                        host=self.host,
                        port=self.port,
                        user=self.user,
                        password=self.password,
                        database=self.dbname,
                        autocommit=True,
                        connect_timeout=3,
                    )
                else:
                    return pymysql.connect(
                        host=self.host,
                        port=self.port,
                        user=self.user,
                        password=self.password,
                        database=self.dbname,
                        autocommit=True,
                        connection_timeout=3,
                    )
            except Exception as e:
                log.warning(f"MariaDB connection failed ({self.host}:{self.port}/{self.dbname}): {e}")
                log.warning("Falling back to local SQLite database...")
                self.use_sqlite = True

        # SQLite connection
        conn = sqlite3.connect(str(self.sqlite_path), timeout=5)
        conn.row_factory = sqlite3.Row
        return conn

    def init_db(self):
        """Create MariaDB/SQLite tables if they do not exist."""
        conn = None
        try:
            conn = self.get_connection()
            cursor = conn.cursor()

            if self.use_sqlite:
                # SQLite table DDLs
                cursor.execute("""
                    CREATE TABLE IF NOT EXISTS hardware_videos (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT UNIQUE NOT NULL,
                        file_path TEXT NOT NULL,
                        size_bytes INTEGER DEFAULT 0,
                        duration_seconds INTEGER DEFAULT 0,
                        play_order INTEGER DEFAULT 0,
                        created_at TEXT
                    )
                """)
                cursor.execute("""
                    CREATE TABLE IF NOT EXISTS hardware_playback_logs (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        video_name TEXT NOT NULL,
                        event_type TEXT NOT NULL,
                        elapsed_seconds INTEGER DEFAULT 0,
                        timestamp_utc TEXT NOT NULL
                    )
                """)
                cursor.execute("""
                    CREATE TABLE IF NOT EXISTS hardware_settings (
                        setting_key TEXT PRIMARY KEY,
                        setting_value TEXT NOT NULL
                    )
                """)
            else:
                # MariaDB / MySQL table DDLs from schema.sql if present
                schema_path = Path(__file__).parent / "schema.sql"
                if schema_path.exists():
                    sql_content = schema_path.read_text(encoding="utf-8")
                    # Split statements by semicolon
                    statements = [stmt.strip() for stmt in sql_content.split(";") if stmt.strip()]
                    for stmt in statements:
                        try:
                            cursor.execute(stmt)
                        except Exception as ex:
                            log.debug(f"Statement execution debug: {ex}")
                else:
                    cursor.execute(f"CREATE DATABASE IF NOT EXISTS `{self.dbname}`")
                    cursor.execute(f"USE `{self.dbname}`")

                    cursor.execute("""
                        CREATE TABLE IF NOT EXISTS hardware_videos (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            name VARCHAR(255) UNIQUE NOT NULL,
                            file_path VARCHAR(500) NOT NULL,
                            size_bytes BIGINT DEFAULT 0,
                            duration_seconds INT DEFAULT 0,
                            play_order INT DEFAULT 0,
                            created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4
                    """)
                    cursor.execute("""
                        CREATE TABLE IF NOT EXISTS hardware_playback_logs (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            video_name VARCHAR(255) NOT NULL,
                            event_type VARCHAR(50) NOT NULL,
                            elapsed_seconds INT DEFAULT 0,
                            timestamp_utc DATETIME DEFAULT CURRENT_TIMESTAMP
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4
                    """)
                cursor.execute("""
                    CREATE TABLE IF NOT EXISTS hardware_settings (
                        setting_key VARCHAR(100) PRIMARY KEY,
                        setting_value TEXT NOT NULL
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4
                """)

            if self.use_sqlite:
                conn.commit()

            mode = "SQLite (" + str(self.sqlite_path) + ")" if self.use_sqlite else f"MariaDB ({self.host}:{self.port}/{self.dbname})"
            log.info(f"✅ Hardware Database initialized in {mode} mode.")

        except Exception as e:
            log.error(f"Database initialization error: {e}")
        finally:
            if conn:
                try: conn.close()
                except Exception: pass

    def sync_videos_to_db(self, video_list: list):
        """Sync scanned media files into MariaDB/SQLite database."""
        conn = None
        try:
            conn = self.get_connection()
            cursor = conn.cursor()
            now = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M:%S")

            for order, v in enumerate(video_list):
                name = v["name"]
                path = v.get("url", f"/media/{name}")
                size = v.get("size_bytes", 0)

                if self.use_sqlite:
                    cursor.execute("""
                        INSERT INTO hardware_videos (name, file_path, size_bytes, play_order, created_at)
                        VALUES (?, ?, ?, ?, ?)
                        ON CONFLICT(name) DO UPDATE SET
                            file_path=excluded.file_path,
                            size_bytes=excluded.size_bytes,
                            play_order=excluded.play_order
                    """, (name, path, size, order, now))
                else:
                    cursor.execute("""
                        INSERT INTO hardware_videos (name, file_path, size_bytes, play_order, created_at)
                        VALUES (%s, %s, %s, %s, %s)
                        ON DUPLICATE KEY UPDATE
                            file_path=VALUES(file_path),
                            size_bytes=VALUES(size_bytes),
                            play_order=VALUES(play_order)
                    """, (name, path, size, order, now))

            if self.use_sqlite:
                conn.commit()

        except Exception as e:
            log.warning(f"Video sync to DB error: {e}")
        finally:
            if conn:
                try: conn.close()
                except Exception: pass

    def log_playback_event(self, video_name: str, event_type: str, elapsed_seconds: int = 0):
        """Record video playback event to MariaDB/SQLite log table."""
        conn = None
        try:
            conn = self.get_connection()
            cursor = conn.cursor()
            now = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M:%S")

            if self.use_sqlite:
                cursor.execute("""
                    INSERT INTO hardware_playback_logs (video_name, event_type, elapsed_seconds, timestamp_utc)
                    VALUES (?, ?, ?, ?)
                """, (video_name, event_type, elapsed_seconds, now))
                conn.commit()
            else:
                cursor.execute("""
                    INSERT INTO hardware_playback_logs (video_name, event_type, elapsed_seconds, timestamp_utc)
                    VALUES (%s, %s, %s, %s)
                """, (video_name, event_type, elapsed_seconds, now))

        except Exception as e:
            log.debug(f"Playback log DB error: {e}")
        finally:
            if conn:
                try: conn.close()
                except Exception: pass

    def get_db_videos(self) -> list:
        """Fetch list of videos stored in MariaDB/SQLite."""
        conn = None
        try:
            conn = self.get_connection()
            cursor = conn.cursor()
            cursor.execute("SELECT name, file_path, size_bytes, duration_seconds FROM hardware_videos ORDER BY play_order ASC")

            rows = cursor.fetchall()
            videos = []
            for row in rows:
                if isinstance(row, dict) or hasattr(row, "keys"):
                    videos.append({
                        "name": row["name"],
                        "url": row["file_path"],
                        "size_bytes": row["size_bytes"],
                        "duration": row["duration_seconds"]
                    })
                else:
                    videos.append({
                        "name": row[0],
                        "url": row[1],
                        "size_bytes": row[2],
                        "duration": row[3]
                    })
            return videos
        except Exception as e:
            log.warning(f"Fetch DB videos error: {e}")
            return []
        finally:
            if conn:
                try: conn.close()
                except Exception: pass
