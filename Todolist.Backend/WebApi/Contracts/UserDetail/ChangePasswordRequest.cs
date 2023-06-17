namespace WebApi.Contracts.UserDetail
{
    public record ChangePasswordRequest(
        string OldPassword,
        string NewPassword);
}
