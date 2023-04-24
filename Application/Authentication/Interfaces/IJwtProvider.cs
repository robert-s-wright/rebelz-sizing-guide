using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Application.Authentication.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(UserModel user, IConfiguration configuration);



    }
}
