using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using static Application.Services.Methods;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSubmissionsController : ControllerBase
    {
        // GET: api/<UserSubmissionsController>/email
        [HttpGet("{email}")]
        public ActionResult GetByEmail(UserModel user)
        {
            bool previousSubmissions = GetUserSubmissions(user);

            if (previousSubmissions)
            {
                return Ok(user);
            }
            else
            {
                return StatusCode(204);
            }
        }

        // POST api/<UserSubmissionsController>
        [HttpPost]
        public ActionResult Post([FromBody] UserSubmissionsModel user)
        {
            PostUserSubmissions(user);

            return Ok(user);
        }
    }
}
