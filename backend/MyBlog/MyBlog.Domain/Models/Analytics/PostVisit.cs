using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models.Analytics
{
    public class PostVisit
    {
        public Guid Id { get;private set; }
        public Guid PostId { get; private set; }

        public string VisitorKey { get; private set; }

        public DateTime VisitedAtUtc { get; private set; }

        public PostVisit(Guid postId,string visitorKey)
        {
            if(postId == Guid.Empty)
            {
                throw new ArgumentException("Yazı Kimliği boş olamaz.", nameof(postId));
            }

            if (string.IsNullOrWhiteSpace(visitorKey))
            {
                throw new ArgumentException("Ziyaretçi anahtarı boş olamaz.", nameof(visitorKey));
            }
            Id = Guid.NewGuid();
            PostId = postId;
            VisitorKey = visitorKey.Trim();
            VisitedAtUtc = DateTime.UtcNow;
        }

    }
}
