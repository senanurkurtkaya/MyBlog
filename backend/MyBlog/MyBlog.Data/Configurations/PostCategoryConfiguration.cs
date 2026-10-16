using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Domain.Models.Categories;
using MyBlog.Domain.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Configurations
{
    internal class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            builder.HasKey(x => new {x.PostId ,x.CategoryId});
            builder.HasOne<Post>().WithMany(x => x.PostCategories).HasForeignKey(x => x.PostId);
            builder.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId);
       
        }
      
    }
}
