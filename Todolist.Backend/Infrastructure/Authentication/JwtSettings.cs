namespace Infrastructure.Authentication
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string? SecretKey { get; init; }
        public int ExpiryInMinutes { get; init; }
        public string? Issuer { get; init; }
        public string? Audience { get; init; }
    }
}
