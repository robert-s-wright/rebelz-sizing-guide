using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration
            )
        {
            string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

            //Cors
            services.AddCors(options =>
            {
                //var origins = configuration.GetSection("AllowedHosts").Get<string[]>();
                options.AddPolicy(myAllowSpecificOrigins,
                    policy =>
                    {
                        var origins = configuration["AllowedHosts"];
                        policy.WithOrigins("http://localhost:3000", "https://localhost:7287").AllowAnyMethod().AllowAnyHeader();
                        policy.WithHeaders().AllowCredentials().WithMethods("GET", "PUT", "POST", "DELETE", "PATCH", "OPTIONS");

                    }
                    );
            });

            //Authentication

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var request = ctx.HttpContext.Request;
                        var cookies = request.Cookies;
                        if (cookies.TryGetValue(configuration["Jwt:AuthTokenKey"], out var accessTokenValue))
                        {
                            ctx.Token = accessTokenValue;
                        }
                        return Task.CompletedTask;
                    },

                };
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    RequireAudience = false,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"])),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                };

            })
                .AddCookie(x =>
                {
                    x.Cookie.HttpOnly = true;
                    x.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                    x.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                    x.Cookie.IsEssential = true;



                });



            return services;
        }
    }
}
