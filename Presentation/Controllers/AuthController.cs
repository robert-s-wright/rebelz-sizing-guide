using Application;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            UserModel user = new UserModel();

            var request = Request;

            var jwtClaims = new JwtSecurityToken(request.Cookies.First().Value).Claims;

            var email = jwtClaims.First(c => c.Type == "email").Value;

            user.Email = email;

            user = GlobalConfig.Connection.GetUser_ByEmail(user);

            user.Password = null;

            return Ok(user);

        }
    }
}
