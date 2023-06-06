namespace WebApi.Contracts.Authentication
{
    public record AuthenticationResponse(
        int Id,
        Guid Uuid,
        string FirstName,
        string LastName,
        string Username,
        string Token);
}
