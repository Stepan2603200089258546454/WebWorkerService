using Microsoft.AspNetCore.Mvc;

namespace WebWorkerService.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        
    }
}
