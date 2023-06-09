﻿namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateTimeUpdated { get; set; } = DateTime.UtcNow;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenCreated { get; set; }
        public DateTime RefreshTokenExpires { get; set; }

        public ICollection<TodoTask>? TodoTasks { get; set; }
    }
}
