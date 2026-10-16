using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace MyBlog.Domain.Models.Comments
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }

        public string AuthorName { get; private set; }
        public string AuthorEmail { get; private set; }
        public string Content { get; private set; }

        public bool IsApproved { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? ApprovedAtUtc { get; private set; }

        public Comment(Guid postId ,string authorName ,string authorEmail ,string content)
        {
            if(postId == Guid.Empty)
            {
                throw new ArgumentException("Yazı Kimliği boş olamaz",nameof(postId));
            }
            if (string.IsNullOrWhiteSpace(authorName))
            {
                throw new ArgumentException("Yorum Yapan kişinin adı boş olamaz",nameof (authorName));
            }
            if(string.IsNullOrWhiteSpace(authorEmail))
            {
                throw new ArgumentException("E-Posta Adresi boş olamaz", nameof(authorEmail));
            }
            var normalizedEmail = authorEmail.Trim();
            if(!MailAddress.TryCreate(normalizedEmail, out var mailAddress))
            {
                throw new ArgumentException("Geçerli bir e-posta adresi girilmelidir", nameof(authorEmail));
            }
            if(mailAddress.Address != normalizedEmail)
            {
                throw new ArgumentException("Yalnızca e-posta adresi girilmelidir.", nameof(authorEmail));
            }
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("yorum metni boş olamaz",nameof(content));
            }
            Id = Guid.NewGuid();
            PostId = postId;
            AuthorName = authorName.Trim();
            AuthorEmail = normalizedEmail;
            Content = content.Trim();
            IsApproved = false;
            CreatedAtUtc = DateTime.UtcNow;
            

        }
        public void Approve()
        {
            if (IsApproved)
            {
                return;
            }
            IsApproved = true;
            ApprovedAtUtc = DateTime.UtcNow;
        }
        public void Unapprove()
        {
            if (!IsApproved)
            {
                return;
            }
            IsApproved = false;
            ApprovedAtUtc = null;
    
        }
    }
}
