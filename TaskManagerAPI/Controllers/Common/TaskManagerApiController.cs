using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Api.Controllers.Common;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class TaskManagerApiController : ControllerBase
{
}
