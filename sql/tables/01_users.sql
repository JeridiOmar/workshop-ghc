-- =============================================
-- Create users table
-- =============================================
-- This table stores user credentials for profile management
-- Username allows spaces and must be unique
-- PIN is stored as a hashed value using BCrypt/PBKDF2

CREATE TABLE users (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    username NVARCHAR(50) NOT NULL UNIQUE,
    pin_hash NVARCHAR(255) NOT NULL,
    created_at DATETIME2 DEFAULT GETUTCDATE()
);

-- Index for faster username lookups during login
CREATE INDEX idx_username ON users(username);
