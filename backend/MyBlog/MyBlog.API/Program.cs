
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Business;
using MyBlog.API.ExceptionHandling;

namespace MyBlog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                     .AddControllers()
                      .ConfigureApiBehaviorOptions(options =>
                      {
                          options.ClientErrorMapping[404].Title = "Kaynak bulunamadý";
                      });
            builder.Services.AddOpenApi();
            builder.Services.AddMyBlogData(builder.Configuration.GetConnectionString("MyBlog"));
            builder.Services.AddMyBlogBusiness();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            
            var app = builder.Build();
            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
