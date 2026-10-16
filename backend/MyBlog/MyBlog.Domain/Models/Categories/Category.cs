using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models.Categories
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get;private set; }
        public string? Color { get; private set; }
        public int DisplayOrder { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? UpdatedAtUtc { get; private set; }

        public Category(string name , string slug )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Kategori Adı Boş Olamaz.");
            }
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Kategori Slug'ı boş olamaz ");
            }
            Id= Guid.NewGuid();
            Name = name.Trim();
            Slug = slug.Trim();
            CreatedAtUtc = DateTime.UtcNow;

        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Yeni Kategori Adı Boş Olamaz.");
            }
            Name = newName.Trim();
            UpdatedAtUtc= DateTime.UtcNow;
        }

        public void ChangeSlug(string newSlug)
        {
            if(string.IsNullOrWhiteSpace(newSlug))
            {
                throw new ArgumentException("Yeni Kategori Slug'ı Boş Olamaz.");
            }
            Slug= newSlug.Trim();
            UpdatedAtUtc= DateTime.UtcNow;
        }
        public void ChangeDisplayOrder(int displayOrder)
        {
            if(0 > displayOrder)
            {
                throw new ArgumentOutOfRangeException(nameof(displayOrder));
            }
            DisplayOrder = displayOrder;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void ChangeColor(string? color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                Color = null;
            }
            else
            {
                var normalizedColor = color.Trim();
                if(normalizedColor.Length != 7 || !normalizedColor.StartsWith("#"))
                {
                    throw new ArgumentException("Renk kodu # ile başlamalı ve 7 karakter olmalıdır.", (nameof(color)));
                }
                for (int i = 1; i < normalizedColor.Length; i++)
                {
                    var character = normalizedColor[i];
                    var isDigit = character >= '0' && character <= '9';
                    var isUpperHex = character >= 'A' && character <= 'F';
                    var isLowerHex = character >= 'a' && character <= 'f';

                    if(!isDigit && !isUpperHex && !isLowerHex)
                    {
                        throw new ArgumentException("Renk kodunda yalnızca 0–9, A–F ve a–f karakterleri kullanılabilir.",nameof(color));
                    }
                }

           
                Color = normalizedColor;
            }
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
