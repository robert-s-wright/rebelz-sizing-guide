using Application;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_ModelController : ControllerBase
    {


        // GET api/<User_ModelController>/5
        [HttpGet("{id}")]
        public ActionResult GetUserModelsById(int id)
        {
            List<User_ModelModel> reviews = GlobalConfig.Connection.GetUserModels_ByUserId(id);

            if (reviews.Count == 0)
            {
                return StatusCode(204);
            }
            else
            {
                return Ok(reviews);
            }
        }

        // POST api/<User_ModelController>
        [HttpPost]
        public ActionResult Post([FromBody] List<User_ModelModel> userModels)
        {
            GlobalConfig.Connection.CreateNewUserModels(userModels);

            return Ok(userModels);
        }

    }
}
