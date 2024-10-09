
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.DbContexts;
using Persistence.IdentityCustoms;
using ProductMVC;
using SignalR.Hubs;
using System.Text;

namespace ChatApis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers()
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                      options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

                  });
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            //Presentation dependencies
            builder.Services.AddPresentationService(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(origin => true)
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });
            //var key = Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]!);
            //var securityKey = builder.Configuration["Token:SecurityKey"];
            //if (string.IsNullOrEmpty(securityKey))
            //{
            //    throw new ArgumentNullException("SecurityKey", "Token SecurityKey is not configured.");
            //}
            var key = Encoding.UTF8.GetBytes("llkwerlkw378246837ijshdfkjshasdasaadasdasdadadsasdasdsadsasdasdad");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero 
                };
            });

            builder.Services.AddHttpContextAccessor();
            //builder.WebHost.ConfigureKestrel(serverOptions =>
            //{
            //    serverOptions.ListenAnyIP(5089); 
            //});

            var app = builder.Build();

            //app.Urls.Add("http://*:5089");
            app.Urls.Add("http://0.0.0.0:5000");

            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapHub<ChatHub>("/api/chatHub");
            app.MapControllers();

            app.Run();
        }
    }
}
