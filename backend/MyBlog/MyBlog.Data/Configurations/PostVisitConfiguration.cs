using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Domain.Models.Analytics;
using MyBlog.Domain.Models.Media;
using MyBlog.Domain.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Configurations
{
    internal class PostVisitConfiguration : IEntityTypeConfiguration<PostVisit>
    {
        public void Configure(EntityTypeBuilder<PostVisit> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.VisitorKey).IsRequired().HasMaxLength(200);

            builder.HasOne<Post>().WithMany().HasForeignKey(x => x.PostId);
            builder.HasIndex(x => new { x.PostId, x.VisitedAtUtc });
           
        }
    }
}
