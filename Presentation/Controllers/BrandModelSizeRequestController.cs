using Domain.Models;
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
            BrandModelSizeRequestModel data = new BrandModelSizeRequestModel();

            BrandModelSizeRequestCompilation(data);

            return Ok(data);
        }


    }
}
