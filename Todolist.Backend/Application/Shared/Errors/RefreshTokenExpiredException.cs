using System.Net;

namespace Application.Shared.Errors
{
    public class RefreshTokenExpiredException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
        public string ErrorDetailMessage => "Refresh token has expired.";
    }
}
