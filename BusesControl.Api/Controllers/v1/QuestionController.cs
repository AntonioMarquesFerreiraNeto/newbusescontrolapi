using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1
{
    [Route("api/v1/question")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
    }
}
