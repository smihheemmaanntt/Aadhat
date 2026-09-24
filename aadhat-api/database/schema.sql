CREATE TABLE IF NOT EXISTS hybrid_companies (
    id INT AUTO_INCREMENT PRIMARY KEY,
    company_code VARCHAR(64) NOT NULL UNIQUE,
    company_name VARCHAR(255) NOT NULL,
    mode ENUM('LocalOnly','OnlineOnly','SyncingToOnline','SyncingToLocal','ReadOnlyArchive') NOT NULL DEFAULT 'LocalOnly',
    local_db_path VARCHAR(500) NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS hybrid_audit_log (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    company_code VARCHAR(64) NOT NULL,
    user_name VARCHAR(100) NULL,
    pc_name VARCHAR(100) NULL,
    action VARCHAR(100) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_company_created (company_code, created_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS hybrid_transfer_jobs (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    company_code VARCHAR(64) NOT NULL,
    direction ENUM('LocalToOnline','OnlineToLocal') NOT NULL,
    status ENUM('Pending','Running','Completed','Failed','Cancelled') NOT NULL DEFAULT 'Pending',
    requested_by VARCHAR(100) NULL,
    requested_pc VARCHAR(100) NULL,
    message TEXT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_company_status (company_code, status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS hybrid_authorization_keys (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    auth_key_hash CHAR(64) NOT NULL UNIQUE,
    customer_code VARCHAR(64) NOT NULL,
    max_devices INT NOT NULL DEFAULT 1,
    status ENUM('Active','Paused','Revoked') NOT NULL DEFAULT 'Active',
    expires_at DATETIME NULL,
    note VARCHAR(255) NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_customer_status (customer_code, status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS hybrid_devices (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    auth_key_id BIGINT NOT NULL,
    customer_code VARCHAR(64) NOT NULL,
    device_id_hash CHAR(64) NOT NULL,
    pc_name VARCHAR(100) NULL,
    device_token_hash CHAR(64) NOT NULL,
    status ENUM('Active','Blocked','Released') NOT NULL DEFAULT 'Active',
    activated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    last_seen_at DATETIME NULL,
    UNIQUE KEY uq_auth_device (auth_key_id, device_id_hash),
    INDEX idx_customer_status (customer_code, status),
    CONSTRAINT fk_hybrid_devices_auth_key FOREIGN KEY (auth_key_id)
        REFERENCES hybrid_authorization_keys(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS hybrid_company_access (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    customer_code VARCHAR(64) NOT NULL,
    company_code VARCHAR(64) NOT NULL,
    can_access TINYINT(1) NOT NULL DEFAULT 1,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_customer_company (customer_code, company_code)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Example:
-- INSERT INTO hybrid_companies (company_code, company_name, mode)
-- VALUES ('1', 'Demo Company', 'LocalOnly')
-- ON DUPLICATE KEY UPDATE company_name = VALUES(company_name);
--
-- INSERT INTO hybrid_company_access (customer_code, company_code, can_access)
-- VALUES ('CUST001', '1', 1)
-- ON DUPLICATE KEY UPDATE can_access = VALUES(can_access);
