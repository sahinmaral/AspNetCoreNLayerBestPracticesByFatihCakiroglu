using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    public interface ICategoryServiceWithDto : IServiceWithDto<Category, CategoryDto>
    {
        Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProducts(int categoryId);
        Task<CustomResponseDto<CategoryUpdateDto>> UpdateAsync(CategoryUpdateDto dto);
        Task<CustomResponseDto<CategoryCreateDto>> AddAsync(CategoryCreateDto dto);
        Task<CustomResponseDto<IEnumerable<CategoryCreateDto>>> AddRangeAsync(IEnumerable<CategoryCreateDto> dtos);

    }
}
