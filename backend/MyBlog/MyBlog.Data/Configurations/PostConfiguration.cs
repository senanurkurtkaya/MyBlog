using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Domain.Models.Media;
using MyBlog.Domain.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Configurations
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            builder.HasIndex(x => x.Slug).IsUnique();
            builder.Property(x=> x.Summary).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.SeoTitle).HasMaxLength(200);
            builder.Property(x =>x.SeoDescription).HasMaxLength(500);
            builder.Property(x =>x.ContentMarkdown).IsRequired();
            builder.Navigation(x => x.PostCategories).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.HasOne<MediaFile>().WithMany().HasForeignKey(x => x.CoverImageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
