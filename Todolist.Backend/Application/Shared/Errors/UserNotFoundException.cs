using System.Net;

namespace Application.Shared.Errors
{
    public class UserNotFoundException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public string ErrorDetailMessage => "User was not found";
    }
}
