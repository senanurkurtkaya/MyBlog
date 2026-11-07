using MyBlog.Business.Categories.Dtos;
using MyBlog.Data.Repositories.Categories;
using MyBlog.Data.UnitOfWork;
using MyBlog.Domain.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid>CreateAsync(CreateCategoryDto dto, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Kategori Adı Boş Olamaz");
            }

            if (string.IsNullOrWhiteSpace(dto.Slug))
            {
                throw new ArgumentException("Kategori Slug'ı boş olamaz.");
            }
            var name = dto.Name.Trim();
            var slug = dto.Slug.Trim();

            if (name.Length > 100)
            {
                throw new ArgumentException("Kategori adı en fazla 100 karakter olabilir");
            }

            if (slug.Length > 200)
            {
                throw new ArgumentException("Kategori slug'ı en fazla 200 karakter olabilir");
            }

           var slugExists = 
            await _categoryRepository.ExistsBySlugAsync(slug ,cancellation);

            if (slugExists)
            {
                throw new InvalidOperationException("Bu Slug zaten kullanılıyor");
            }
            var category = new Category(name ,slug);

            category.ChangeColor(dto.Color);
            category.ChangeDisplayOrder(dto.DisplayOrder);

         _categoryRepository.Add(category);
        await _unitOfWork.SaveChangesAsync(cancellation);
            return category.Id;

        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
          var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                return false;
            }
            _categoryRepository.Remove(category);
          await _unitOfWork.SaveChangesAsync(cancellationToken);
          return true;
        }


        public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            return categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Color = x.Color,
                DisplayOrder = x.DisplayOrder,
            }).ToList();

        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
           var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

            if (category == null)
            {
                return null;
            }

            var dto = new CategoryDto();
            dto.Id = category.Id;
            dto.Name =category.Name;
            dto.Slug =category.Slug;
            dto.Color = category.Color;
            dto.DisplayOrder = category.DisplayOrder;
            return dto;

        }

        public async Task<bool> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Kategori adı boş olamaz");
            }

            if (string.IsNullOrWhiteSpace(dto.Slug))
            {
                throw new ArgumentException("Kategori slug'ı boş olamaz");
            }
            var name = dto.Name.Trim();
            var slug = dto.Slug.Trim();
            
            if(name.Length > 100)
            {
                throw new ArgumentException("Kategori adı en fazla 100 karakter olabilir");
            } 

            if(slug.Length > 200)
            {
                throw new ArgumentException("Kategori slug'ı en fazla 200 karakter olabilir");
            }
          
            var category = await   _categoryRepository.GetByIdAsync(id, cancellationToken);

            if(category == null)
            {
                return false;
            }

           
            if (category.Slug != slug)
            {

                var slugExists = await _categoryRepository.ExistsBySlugAsync(slug, cancellationToken);

                if (slugExists)
                {
                    throw new InvalidOperationException("Bu Slug zaten kullanılıyor");
                }
                    category.ChangeSlug(slug);
                
            }
            category.ChangeName(name);
            category.ChangeColor(dto.Color);
            category.ChangeDisplayOrder(dto.DisplayOrder);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;

        }     
  
    }
}
