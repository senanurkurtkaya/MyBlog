using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddMyBlogData(
                builder.Configuration.GetConnectionString("MyBlog"));
            builder.Services.AddMyBlogBusiness();

            var app = builder.Build();

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