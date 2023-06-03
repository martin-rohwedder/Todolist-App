using Application.Shared.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [Route("/Error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            var (statusCode, message) = exception switch
            {
                IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorDetailMessage),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occured."),
            };

            return Problem(statusCode: statusCode, detail: message);
        }
    }
}
