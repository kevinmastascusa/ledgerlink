using Microsoft.AspNetCore.Mvc;

namespace LedgerLink.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected ActionResult<T> HandleResult<T>(T result)
        {
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        protected ActionResult HandleResult(bool success)
        {
            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }

        protected ActionResult HandleException(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 