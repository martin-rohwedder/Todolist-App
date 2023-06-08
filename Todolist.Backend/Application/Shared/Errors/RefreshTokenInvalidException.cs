using System.Net;

namespace Application.Shared.Errors
{
    public class RefreshTokenInvalidException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
        public string ErrorDetailMessage => "Refresh token is invalid.";
    }
}
