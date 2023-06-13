using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Application.Shared.Errors
{
    [ExcludeFromCodeCoverage]
    public class UserNotFoundException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public string ErrorDetailMessage => "User was not found";
    }
}
