using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Data.Contexts;
using MyBlog.Data.Repositories.Categories;
using MyBlog.Data.Repositories.Posts;
using MyBlog.Data.UnitOfWork;

namespace MyBlog.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMyBlogData(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<AddMyBlogData>(options =>
    options.UseNpgsql(connectionString));
            
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUnitOfWork, MyBlog.Data.UnitOfWork.UnitOfWork>();
            return services;
        }

    }
}