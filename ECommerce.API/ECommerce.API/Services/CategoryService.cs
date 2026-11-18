using ECommerce.API.DTOs.Category;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using ECommerce.API.Services.Interfaces;

namespace ECommerce.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => new CategoryResponseDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return null;
            }

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {
            string name = dto.Name.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.");
            }

            if (await _categoryRepository.NameExistsAsync(name))
            {
                throw new ArgumentException("Category name already exists.");
            }

            var category = new Category
            {
                Name = name,
                Description = dto.Description.Trim()
            };

            await _categoryRepository.AddAsync(category);

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryResponseDto?> UpdateAsync(
            int id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return null;
            }

            string name = dto.Name.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.");
            }

            if (await _categoryRepository.NameExistsAsync(name, id))
            {
                throw new ArgumentException("Category name already exists.");
            }

            category.Name = name;
            category.Description = dto.Description.Trim();

            await _categoryRepository.UpdateAsync(category);

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return false;
            }

            if (await _categoryRepository.HasProductsAsync(id))
            {
                throw new InvalidOperationException(
                    "Cannot delete category because products are linked to it.");
            }

            await _categoryRepository.DeleteAsync(category);

            return true;
        }
    }
}