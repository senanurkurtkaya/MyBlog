using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Models.Analytics;
using MyBlog.Domain.Models.Categories;
using MyBlog.Domain.Models.Comments;
using MyBlog.Domain.Models.Media;
using MyBlog.Domain.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Contexts
{
    public class MyBlogDbContext:DbContext
    {
        public MyBlogDbContext(DbContextOptions<MyBlogDbContext> options ):base(options)
        {

        }
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<PostCategory> PostCategories => Set<PostCategory>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
        public DbSet<PostVisit> PostVisits => Set<PostVisit>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyBlogDbContext).Assembly);
        }
    }
}
