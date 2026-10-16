using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Domain.Models.Comments;
using MyBlog.Domain.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AuthorName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.AuthorEmail).IsRequired().HasMaxLength(254);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(5000);

            builder.HasOne<Post>().WithMany().HasForeignKey(x => x.PostId);
        }
    }
}
