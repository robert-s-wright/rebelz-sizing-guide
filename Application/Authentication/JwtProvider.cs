using Application.Authentication.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Authentication
{

    public class JwtProvider : IJwtProvider
    {
        public JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();


        private static SymmetricSecurityKey _key;
        private static SigningCredentials _creds;
        public IConfiguration _configuration;

        public JwtProvider(IConfiguration config)
        {
            _configuration = config;
            _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]));
            _creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        }



        public string Generate(UserModel user, IConfiguration configuration)
        {

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            List<Claim> claims = new List<Claim>
                {
                new Claim(ClaimTypes.Email, user.Email)
            };


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                //IssuedAt = DateTime.UtcNow,
                //Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = _creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return _jwtSecurityTokenHandler.WriteToken(token);


        }


    }
}
