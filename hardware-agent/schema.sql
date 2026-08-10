-- ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
-- HBeonLabs Video Display Platform — MariaDB / MySQL Schema
-- Enterprise Hardware Database Setup
-- ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

CREATE DATABASE IF NOT EXISTS `videodisplaydb` 
  DEFAULT CHARACTER SET utf8mb4 
  DEFAULT COLLATE utf8mb4_unicode_ci;

USE `videodisplaydb`;

-- 1. Hardware Device Master Table
CREATE TABLE IF NOT EXISTS `hardware_devices` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `device_id` VARCHAR(100) UNIQUE NOT NULL,
    `hw_uid` VARCHAR(100) NOT NULL,
    `mac_address` VARCHAR(50) NOT NULL,
    `ip_address` VARCHAR(50) NOT NULL,
    `hostname` VARCHAR(100) NOT NULL,
    `platform` VARCHAR(50) DEFAULT 'Linux',
    `status` VARCHAR(20) DEFAULT 'ONLINE',
    `current_volume` INT DEFAULT 80,
    `current_brightness` INT DEFAULT 80,
    `is_display_on` TINYINT(1) DEFAULT 1,
    `created_at` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `last_seen_utc` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX `idx_device_mac` (`mac_address`),
    INDEX `idx_device_status` (`status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 2. Hardware Video Library & Playlist Table
CREATE TABLE IF NOT EXISTS `hardware_videos` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `device_id` VARCHAR(100) DEFAULT 'ALL',
    `name` VARCHAR(255) UNIQUE NOT NULL,
    `file_path` VARCHAR(500) NOT NULL,
    `size_bytes` BIGINT DEFAULT 0,
    `duration_seconds` INT DEFAULT 0,
    `play_order` INT DEFAULT 0,
    `is_active` TINYINT(1) DEFAULT 1,
    `loop_count` INT DEFAULT 0,
    `created_at` DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX `idx_video_order` (`play_order`),
    INDEX `idx_video_active` (`is_active`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 3. Live Playback Event Logs Table
CREATE TABLE IF NOT EXISTS `hardware_playback_logs` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `device_id` VARCHAR(100) NOT NULL,
    `video_name` VARCHAR(255) NOT NULL,
    `event_type` VARCHAR(50) NOT NULL, -- PLAY, PAUSE, STOP, ENDED, ERROR
    `elapsed_seconds` INT DEFAULT 0,
    `timestamp_utc` DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX `idx_log_device` (`device_id`),
    INDEX `idx_log_timestamp` (`timestamp_utc`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 4. Hardware Health Telemetry Logs Table
CREATE TABLE IF NOT EXISTS `hardware_telemetry_logs` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `device_id` VARCHAR(100) NOT NULL,
    `cpu_percent` DECIMAL(5,2) DEFAULT 0.00,
    `memory_percent` DECIMAL(5,2) DEFAULT 0.00,
    `storage_percent` DECIMAL(5,2) DEFAULT 0.00,
    `temperature_c` DECIMAL(5,2) DEFAULT 0.00,
    `uptime_seconds` BIGINT DEFAULT 0,
    `signal_strength` INT DEFAULT -60,
    `playback_state` VARCHAR(20) DEFAULT 'IDLE',
    `current_video` VARCHAR(255) DEFAULT NULL,
    `timestamp_utc` DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX `idx_telemetry_device` (`device_id`),
    INDEX `idx_telemetry_time` (`timestamp_utc`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 5. Enterprise Hardware Configuration Key-Value Table
CREATE TABLE IF NOT EXISTS `hardware_settings` (
    `setting_key` VARCHAR(100) PRIMARY KEY,
    `setting_value` TEXT NOT NULL,
    `updated_at` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Seed Default Settings
INSERT INTO `hardware_settings` (`setting_key`, `setting_value`)
VALUES 
    ('app_name', 'HBeonLabs Video Display Platform'),
    ('version', '3.0.0'),
    ('mqtt_broker', '66.116.227.217'),
    ('mqtt_port', '1883'),
    ('auto_loop', 'true'),
    ('default_volume', '80')
ON DUPLICATE KEY UPDATE `updated_at` = CURRENT_TIMESTAMP;
