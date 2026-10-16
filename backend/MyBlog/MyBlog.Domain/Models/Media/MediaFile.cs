using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models.Media
{
    public class MediaFile
    {
        public Guid Id { get;private set; }
        public string OriginalFileName { get; private set; }
        public string StorageKey { get; private set; }
        public string ContentType { get; private set; }
        public long SizeInBytes { get; private set; }

        public string? AltText { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? UpdatedAtUtc { get; private set; }


        public MediaFile(string originalFileName, string storageKey, string contentType,long sizeInBytes ,string? altText)
        {
            if (string.IsNullOrWhiteSpace(originalFileName))
            {
                throw new ArgumentException("Dosya Adı Boş Olamaz.",nameof(originalFileName));
            }
            if (string.IsNullOrWhiteSpace(storageKey))
            {
                throw new ArgumentException("Depolama anahtarı boş olamaz.", nameof(storageKey));
            }
            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentException("Dosya Türü Boş olamaz.",nameof(contentType));
            }
            if(sizeInBytes <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sizeInBytes),"Dosya boyutu sıfırdan büyük olmalıdır.");
            }
            Id = Guid.NewGuid();
            OriginalFileName = originalFileName.Trim();
            StorageKey = storageKey.Trim();
            ContentType = contentType.Trim();
            SizeInBytes = sizeInBytes;

            if (string.IsNullOrWhiteSpace(altText))
            {
                AltText = null; 
            }
            else
            {
                AltText = altText.Trim();
            }
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void ChangeAltText(string? altText)
        {
            if (string.IsNullOrWhiteSpace(altText))
            {
                AltText = null;
            }
            else
            {
                AltText = altText.Trim();
            }
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
