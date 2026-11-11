
using MyBlog.Business.Categories;
using MyBlog.Business.Posts;
using MyBlog.Data;
namespace MyBlog.AdminAPI

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
