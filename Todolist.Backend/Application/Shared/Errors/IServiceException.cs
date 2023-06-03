using System.Net;

namespace Application.Shared.Errors
{
    public interface IServiceException
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorDetailMessage { get; }
    }
}
