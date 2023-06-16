namespace WebApi.Contracts.UserDetail
{
    public record UserDetailResponse(
        Guid Uuid,
        string FirstName,
        string LastName,
        string Username);
}
