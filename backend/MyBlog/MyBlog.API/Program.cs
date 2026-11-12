
using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Categories;
using MyBlog.Business.Posts;
using MyBlog.Data;

namespace MyBlog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddMyBlogData(builder.Configuration.GetConnectionString("MyBlog"));
            
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IPostService, PostService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
