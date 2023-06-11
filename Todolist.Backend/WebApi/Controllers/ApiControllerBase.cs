using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public class ApiControllerBase : ControllerBase
    {
        protected const string ControllerRoutePath = "Api/[controller]/";
    }
}
