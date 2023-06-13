using Microsoft.AspNetCore.Mvc;
using static Application.Services.Methods;


namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandModelSizeRequestController : ControllerBase
    {
        // GET: api/<BrandModelSizeRequestController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(BrandModelSizeRequestCompilation());
        }
    }
}
