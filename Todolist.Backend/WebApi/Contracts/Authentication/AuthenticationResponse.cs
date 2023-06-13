namespace WebApi.Contracts.Authentication
{
    public record AuthenticationResponse(
        Guid Uuid,
        string FirstName,
        string LastName,
        string Username,
        string Token);
}
