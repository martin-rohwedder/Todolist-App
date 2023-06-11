namespace WebApi.Contracts.Authentication
{
    public record AuthenticationResponse(
        int UserId,
        Guid Uuid,
        string FirstName,
        string LastName,
        string Username,
        string Token);
}
