using Application;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using static Application.Services.Methods;

namespace Presentation.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult Login()
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
        [Authorize]
        [HttpGet("logout")]
        public ActionResult Logout()
        {
            var cookies = Request.Cookies.FirstOrDefault(cookie => cookie.Key == "authToken");

            if (cookies.Value != null)
            {
                Response.Cookies.Append("authToken", "expired", new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                    Expires = new DateTime(1970, 1, 1, 0, 0, 0),
                    IsEssential = true,
                });
            }

            return Ok();
        }

        [HttpPost("generate/{email}")]
        public ActionResult ResetPassword(string email)
        {
            User_RecoveryModel userRecoveryModel = CreateUserRecoveryHash(email);




            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Robbie Wright", "rs.wright@yahoo.com"));
            message.To.Add(new MailboxAddress("Robbie Wright", email));
            message.Subject = $"Hey Robbie, here is the password recovery link you requested";
            message.Body = new TextPart("plain")
            {
                Text = "Please follow the link below to reset your password" +
                Environment.NewLine +
                $"http://localhost:3000/reset/{userRecoveryModel.UserId}/{userRecoveryModel.Hash}" +
                Environment.NewLine +
                $"This link will only be valid for 30 minutes, do not share it with anyone!"


            };



            message.WriteTo("testEmail");

            //using (var client = new SmtpClient())
            //{
            //    //client.Connect("localhost");

            //    client.Send(message);
            //    //client.Disconnect(true);
            //}



            return Ok();
        }

        [HttpPatch]

        public ActionResult RecoverPassword(User_RecoveryModel recoveryModel)
        {

            User_RecoveryModel storedRecoveryModel = GlobalConfig.Connection.GetUserRecoveryModel(recoveryModel.UserId);

            if (storedRecoveryModel != null)
            {
                var keyVerification = VerifyUserRecoveryHash(recoveryModel, storedRecoveryModel);

                if (keyVerification)
                {


                    UpdateUserPassword(recoveryModel.UserId, recoveryModel.Password);
                    return Ok();
                }
            }



            return StatusCode(400, "Invalid or expired key");

        }

    }
}
