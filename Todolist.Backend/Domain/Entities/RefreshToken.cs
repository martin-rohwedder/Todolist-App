namespace Domain.Entities
{
    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
        public DateTime Expires { get; set; }
    }
}
