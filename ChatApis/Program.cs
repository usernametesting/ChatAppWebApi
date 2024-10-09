
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

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowOrigin",
            //        builder =>
            //        {
            //            builder.SetIsOriginAllowed(origin => true)
            //                   .AllowAnyMethod()
            //                   .AllowAnyHeader()
            //                   .AllowCredentials();
            //        });
            //});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://parvinnn.netlify.app")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            //var key = Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]!);
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("llkwerlkw378246837ijshdfkjshasdasaadasdasdadadsasdasdsadsasdasdad")),
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero 
                };
            });

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            //builder.WebHost.UseUrls("http://*:5089");
            app.Urls.Add("https://0.0.0.0:5000");


            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapHub<ChatHub>("/api/chatHub");
            app.MapControllers();

            app.Run();
        }
    }
}
