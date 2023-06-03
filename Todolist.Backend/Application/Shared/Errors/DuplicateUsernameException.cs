using System.Net;

namespace Application.Shared.Errors
{
    public class DuplicateUsernameException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public string ErrorDetailMessage => "A user with that username already exists.";
    }
}
