using Core.Enities;
using Microsoft.AspNetCore.Mvc;

namespace Shop_App.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized("Unauthorized");
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("Bad request");
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            return NotFound("Not found");
        }

        [HttpGet("internalerror")]
        public ActionResult GetInternalError()
        {
            throw new Exception();
        }

        [HttpGet("validationerror")]
        public ActionResult GetValidationError(Product product)
        {
            return Ok();
        }

    }
}
