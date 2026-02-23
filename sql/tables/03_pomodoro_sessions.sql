-- =============================================
-- Create pomodoro_sessions table
-- =============================================
-- This table stores all Pomodoro sessions (completed AND incomplete/stopped)
-- duration_seconds reflects actual time spent, even if session was stopped early
-- is_completed flag indicates whether the full planned session was completed

CREATE TABLE pomodoro_sessions (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id UNIQUEIDENTIFIER NOT NULL,
    session_type NVARCHAR(20) NOT NULL,
    duration_seconds INT NOT NULL,
    is_completed BIT NOT NULL DEFAULT 0,
    completed_at DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_pomodoro_sessions_users FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

-- Composite index for efficient queries on user's sessions sorted by date
CREATE INDEX idx_user_completed ON pomodoro_sessions(user_id, completed_at);

-- Index for ranking calculations (sum of duration by user)
CREATE INDEX idx_user_duration ON pomodoro_sessions(user_id, duration_seconds);

-- Constraint to ensure valid session types
ALTER TABLE pomodoro_sessions ADD CONSTRAINT CHK_session_type 
    CHECK (session_type IN ('pomodoro', 'short_break', 'long_break'));

-- Constraint to ensure positive duration
ALTER TABLE pomodoro_sessions ADD CONSTRAINT CHK_duration_positive 
    CHECK (duration_seconds >= 0);
