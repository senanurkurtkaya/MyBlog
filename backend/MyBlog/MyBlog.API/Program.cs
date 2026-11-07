
using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Categories;
using MyBlog.Business.Posts;
using MyBlog.Data.Contexts;
using MyBlog.Data.Repositories.Categories;
using MyBlog.Data.Repositories.Posts;
using MyBlog.Data.UnitOfWork;

namespace MyBlog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<MyBlogDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("MyBlog")));
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IPostRepository,PostRepository>();
            builder.Services.AddScoped<IPostService, PostService>();
            var app = builder.Build();

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
