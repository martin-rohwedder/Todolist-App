namespace Domain.Entities
{
    public class TodoTask
    {
        public int TodoTaskId { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public string Message { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateTimeUpdated { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
    }
}
