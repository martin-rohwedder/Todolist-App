using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Application.Shared.Errors
{
    [ExcludeFromCodeCoverage]
    public class ChangePasswordMismatchException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public string ErrorDetailMessage => "Old password provided does not match with current password.";
    }
}
