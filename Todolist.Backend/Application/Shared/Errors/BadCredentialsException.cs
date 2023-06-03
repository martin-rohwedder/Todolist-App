using System.Net;

namespace Application.Shared.Errors
{
    public class BadCredentialsException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
        public string ErrorDetailMessage => "Bad credentials has been provided.";
    }
}
