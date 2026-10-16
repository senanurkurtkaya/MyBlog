using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Configurations
{
    internal class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> builder)
        {
           builder.HasKey(x => x.Id);
            builder.Property(x => x.OriginalFileName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.StorageKey).IsRequired().HasMaxLength(500);
            builder.Property(x =>x.ContentType).IsRequired().HasMaxLength(100);
            builder.Property(x => x.AltText).HasMaxLength(500);

            builder.HasIndex(x => x.StorageKey).IsUnique();
        }
    }
}
