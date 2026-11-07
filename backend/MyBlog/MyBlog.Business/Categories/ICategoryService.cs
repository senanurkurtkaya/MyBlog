using MyBlog.Business.Categories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Categories
{
    public interface ICategoryService
    {
        public Task<Guid> CreateAsync(CreateCategoryDto dto, CancellationToken cancellation = default);
        public Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default );

        public Task<bool> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        

    }
}
