using Application;
using Application.Authentication;
using Application.Authentication.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using static Application.Services.Methods;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IJwtProvider _jwtProvider;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtProvider = new JwtProvider(configuration);
        }



        //GET api/<UserController>/email
        [HttpGet]
        public ActionResult<UserModel> Get(UserModel user)
        {
            if (user.Email != null)
            {
                UserModel output = GlobalConfig.Connection.GetUser_ByEmail(user);
                return Ok(output);
            }
            else
            {
                return NoContent();
            }
        }


        // Patch api/<UserController>
        [HttpPatch]
        public ActionResult Patch([FromBody] UserModel user)
        {
            UserModel existing = GlobalConfig.Connection.GetUser_ByEmail(user);
            if (existing.Name != user.Name)
            {
                GlobalConfig.Connection.UpdateUser(user);
                return Ok(user);
            }
            else return StatusCode(304);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("Register")]
        // POST api/<UserController>/Register
        [HttpPost]
        public ActionResult<UserModel> Register([FromBody] UserModel user)
        {
            user = GlobalConfig.Connection.GetUser_ByEmail(user);

            if (user.Id != null)
            {
                user.Password = null;
                return StatusCode(409, user);
            }

            else
            {
                RegisterUser(user);
                return Ok(user);
            }

        }


        [Route("Login")]
        // POST api/<UserController>/Login
        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = HttpResponse)]

        public ActionResult Login([FromBody] UserModel user)
        {

            UserModel storedUser = GlobalConfig.Connection.GetUser_ByEmail(user);



            if (storedUser.Id == null)
            {
                return StatusCode(401);


            }
            else
            {

                bool loginSuccess = LoginUser(user, storedUser, _configuration);
                if (loginSuccess)
                {

                    string jwt = _jwtProvider.Generate(user, _configuration);
                    Response.Cookies.Append(_configuration["Jwt:AuthTokenKey"], jwt, new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(1),
                        IsEssential = true,



                    });

                    //GlobalConfig.Connection.GetUserModels_ByUserId(storedUser.Id);
                    storedUser.Password = null;

                    return Ok(storedUser);

                }
                else
                {
                    return StatusCode(401);


                }
            }

        }

        [Route("Admin")]
        // POST api/<UserController>/Admin
        [HttpPost]

        public ActionResult AdminLogin([FromBody] UserModel user)
        {
            UserModel storedUser = GlobalConfig.Connection.GetUser_ByEmail(user);

            if (storedUser.Admin == true)
            {
                bool loginSuccess = LoginUser(user, storedUser, _configuration);
                if (loginSuccess)
                {
                    storedUser.Password = null;
                    return Ok(storedUser);
                }
                else return StatusCode(403);
            }
            else
            {
                return StatusCode(401);
            }
        }

    }
}
