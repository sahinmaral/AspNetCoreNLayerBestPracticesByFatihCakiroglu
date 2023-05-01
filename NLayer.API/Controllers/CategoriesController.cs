using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class CategoriesController : CustomBaseController
    {

        private readonly ICategoryService _categoryService;
        private readonly IService<Category> _service;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IService<Category> service, IMapper mapper)
        {
            _categoryService = categoryService;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("[action]/{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int id)
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProducts(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var categories = await _service.GetAllAsync();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories.ToList());

            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200, categoryDtos));
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var category = await _service.GetByIdAsync(id);
            var categoryDto = _mapper.Map<CategoryDto>(category);

            return CreateActionResult(CustomResponseDto<CategoryDto>.Success(200, categoryDto));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CategoryDto categoryDto)
        {
            var category = await _service.AddAsync(_mapper.Map<Category>(categoryDto));
            var createdCategoryDto = _mapper.Map<CategoryDto>(category);

            return CreateActionResult(CustomResponseDto<CategoryDto>.Success(201, createdCategoryDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> UpdateAsync(CategoryUpdateDto categoryDto)
        {
            await _service.UpdateAsync(_mapper.Map<Category>(categoryDto));

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
