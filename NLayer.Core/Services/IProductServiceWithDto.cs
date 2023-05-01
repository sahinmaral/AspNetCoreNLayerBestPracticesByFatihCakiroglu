using NLayer.Core.DTOs;
using NLayer.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IProductServiceWithDto : IServiceWithDto<Product,ProductDto>
    {
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory();
        Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto);
        Task<CustomResponseDto<ProductCreateDto>> AddAsync(ProductCreateDto dto);
        Task<CustomResponseDto<IEnumerable<ProductCreateDto>>> AddRangeAsync(IEnumerable<ProductCreateDto> dtos);
    }
}
