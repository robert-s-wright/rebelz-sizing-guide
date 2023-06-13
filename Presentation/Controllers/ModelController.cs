using Application;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : Controller
    {

        // POST: ModelController
        [HttpPost]
        public ActionResult PostNewModel([FromBody] ModelModel newModel)
        {
            GlobalConfig.Connection.AddNewModel(newModel);

            return Ok(newModel);
        }


    }
}
