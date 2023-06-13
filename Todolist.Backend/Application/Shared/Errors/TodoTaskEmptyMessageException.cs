using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Application.Shared.Errors
{
    [ExcludeFromCodeCoverage]
    public class TodoTaskEmptyMessageException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string ErrorDetailMessage => "The message of the task is empty";
    }
}
