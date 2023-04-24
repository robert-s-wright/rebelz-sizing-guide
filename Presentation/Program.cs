using Application;
using Application.DataAccess;
using Infrastructure;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GlobalConfig.InitializeConnections(DatabaseType.Sql);

            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            //builder.Services.ConfigureOptions<JwtOptionsSetup>();
            //builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();



            //builder.Services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromSeconds(5);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            //});

            //builder.Services.AddCookiePolicy(options =>
            //{
            //    options.Secure = CookieSecurePolicy.Always;
            //    options.HttpOnly = HttpOnlyPolicy.Always;

            //});


            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddControllers();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app Cors
            app.UseCors("_myAllowSpecificOrigins");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();




            //app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}