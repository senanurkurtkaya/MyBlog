using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyBlog.AdminAPI.ExceptionHandling;
using MyBlog.Business;
using MyBlog.Data;

namespace MyBlog.AdminAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority =
                        builder.Configuration["Keycloak:Authority"];

                    options.Audience =
                        builder.Configuration["Keycloak:Audience"];

                    options.RequireHttpsMetadata =
                        !builder.Environment.IsDevelopment();

                    options.MapInboundClaims = false;
                    options.TokenValidationParameters.RoleClaimType = "roles";
                });

            builder.Services.AddAuthorization();
            builder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                     options.ClientErrorMapping[404].Title = "Kaynak bulunamadý";
                });
            builder.Services.AddOpenApi();

            builder.Services.AddMyBlogData(
                builder.Configuration.GetConnectionString("MyBlog"));
            builder.Services.AddMyBlogBusiness();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var app = builder.Build();
            app.UseExceptionHandler();


            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}