using Application;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelSizeController : ControllerBase
    {


        // POST api/<ModelSizeController>
        [HttpPost]
        public ActionResult<List<Model_SizeModel>> Post([FromBody] List<Model_SizeModel> modelSizes)
        {
            GlobalConfig.Connection.AddModelSizeModels(modelSizes);

            return Ok(modelSizes);
        }


    }
}
