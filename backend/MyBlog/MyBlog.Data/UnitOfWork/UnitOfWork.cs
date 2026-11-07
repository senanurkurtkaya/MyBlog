using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyBlogDbContext _dbContext;

        public UnitOfWork(MyBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
