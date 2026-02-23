-- =============================================
-- Create user_settings table
-- =============================================
-- This table stores user preferences and customization settings
-- One-to-one relationship with users table

CREATE TABLE user_settings (
    user_id UNIQUEIDENTIFIER PRIMARY KEY,
    public_username NVARCHAR(50) NULL, -- Display name for rankings (NULL = use login username)
    pomodoro_duration INT NOT NULL DEFAULT 25,
    short_break_duration INT NOT NULL DEFAULT 5,
    long_break_duration INT NOT NULL DEFAULT 15,
    notifications_enabled BIT NOT NULL DEFAULT 1,
    notification_sound NVARCHAR(50) NOT NULL DEFAULT 'bell',
    notification_volume INT NOT NULL DEFAULT 80,
    theme_focus NVARCHAR(7) NOT NULL DEFAULT '#BA4949',
    theme_short_break NVARCHAR(7) NOT NULL DEFAULT '#38858A',
    theme_long_break NVARCHAR(7) NOT NULL DEFAULT '#397097',
    auto_start_breaks BIT NOT NULL DEFAULT 0,
    auto_start_pomodoro BIT NOT NULL DEFAULT 0,
    long_break_interval INT NOT NULL DEFAULT 4,
    updated_at DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_user_settings_users FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);