using MyBlog.Domain.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Repositories.Categories
{
    public interface ICategoryRepository
    {
        public Task<bool> ExistsBySlugAsync(string slug ,CancellationToken cancellationToken = default);


        public void Add(Category category);

        public void Remove(Category category);

        public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<Category?> GetByIdAsync(Guid id ,CancellationToken cancellationToken =default);
    }
}
