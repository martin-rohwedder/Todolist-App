using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Application.Shared.Errors
{
    [ExcludeFromCodeCoverage]
    public class BadCredentialsException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
        public string ErrorDetailMessage => "Bad credentials has been provided.";
    }
}
