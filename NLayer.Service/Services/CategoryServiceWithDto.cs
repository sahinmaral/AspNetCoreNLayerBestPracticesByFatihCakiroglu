using AutoMapper;

using Microsoft.AspNetCore.Http;

using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class CategoryServiceWithDto : ServiceWithDto<Category, CategoryDto>, ICategoryServiceWithDto
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryServiceWithDto(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository) : base(repository, unitOfWork, mapper)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CustomResponseDto<CategoryCreateDto>> AddAsync(CategoryCreateDto dto)
        {
            Category category = _mapper.Map<Category>(dto);
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.CommitAsync();

            CategoryCreateDto newDto = _mapper.Map<CategoryCreateDto>(category);
            return CustomResponseDto<CategoryCreateDto>.Success(StatusCodes.Status204NoContent, newDto);
        }

        public async Task<CustomResponseDto<IEnumerable<CategoryCreateDto>>> AddRangeAsync(IEnumerable<CategoryCreateDto> dtos)
        {
            IEnumerable<Category> categories = _mapper.Map<IEnumerable<Category>>(dtos);
            await _categoryRepository.AddRangeAsync(categories);
            await _unitOfWork.CommitAsync();

            IEnumerable<CategoryCreateDto> newDtos = _mapper.Map<IEnumerable<CategoryCreateDto>>(categories);
            return CustomResponseDto<IEnumerable<CategoryCreateDto>>.Success(StatusCodes.Status204NoContent, newDtos);
        }

        public async Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProducts(int categoryId)
        {
            Category category = await _categoryRepository.GetSingleCategoryByIdWithProducts(categoryId);
            CategoryWithProductsDto dto = _mapper.Map<CategoryWithProductsDto>(category);
            return CustomResponseDto<CategoryWithProductsDto>.Success(StatusCodes.Status200OK, dto);
        }

        public async Task<CustomResponseDto<CategoryUpdateDto>> UpdateAsync(CategoryUpdateDto dto)
        {
            Category category = _mapper.Map<Category>(dto);
            _categoryRepository.Update(category);
            await _unitOfWork.CommitAsync();

            CategoryUpdateDto updatedDto = _mapper.Map<CategoryUpdateDto>(category);
            return CustomResponseDto<CategoryUpdateDto>.Success(StatusCodes.Status204NoContent,updatedDto);
        }
    }
}
