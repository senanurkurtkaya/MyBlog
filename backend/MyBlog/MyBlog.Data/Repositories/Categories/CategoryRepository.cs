using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Contexts;
using MyBlog.Domain.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyBlogDbContext _dbContext;

        public CategoryRepository(MyBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Category category)
        {
           _dbContext.Categories.Add(category);
        }

 
        public Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return _dbContext.Categories.AnyAsync(x => x.Slug == slug , cancellationToken);
        }

        public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.Categories.AsNoTracking().OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name).ToListAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
           return await _dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        }

        public void Remove(Category category)
        {
            _dbContext.Categories.Remove(category);
        }
    }
}
