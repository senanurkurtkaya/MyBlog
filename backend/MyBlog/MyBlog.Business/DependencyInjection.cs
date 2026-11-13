using Microsoft.Extensions.DependencyInjection;
using MyBlog.Business.Categories;
using MyBlog.Business.Posts;

namespace MyBlog.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMyBlogBusiness(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPostService, PostService>();
            return services;
        }
    }
}
