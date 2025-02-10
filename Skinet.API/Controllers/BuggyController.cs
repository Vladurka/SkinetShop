using Core.Enities;
using Microsoft.AspNetCore.Mvc;

namespace Skinet.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorized() =>
            Unauthorized("Unauthorized");

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest() =>
            BadRequest("Bad request");

        [HttpGet("notfound")]
        public ActionResult GetNotFound() =>
            NotFound("Not found");

        [HttpGet("internalerror")]
        public ActionResult GetInternalError() =>
            throw new Exception();

        [HttpPost("validationerror")]
        public ActionResult GetValidationError(Product product) =>
            BadRequest();
    }
}
