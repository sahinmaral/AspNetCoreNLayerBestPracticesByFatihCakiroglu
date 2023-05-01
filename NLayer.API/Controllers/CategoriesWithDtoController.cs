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
    public class CategoriesWithDtoController : CustomBaseController
    {

        private readonly ICategoryServiceWithDto _categoryService;

        public CategoriesWithDtoController(ICategoryServiceWithDto categoryService)
        {
            _categoryService = categoryService;
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
            return CreateActionResult(await _categoryService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return CreateActionResult(await _categoryService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CategoryDto categoryDto)
        {
            return CreateActionResult(await _categoryService.AddAsync(categoryDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> UpdateAsync(CategoryUpdateDto categoryDto)
        {
            return CreateActionResult(await _categoryService.UpdateAsync(categoryDto));
        }

        [HttpDelete("DeleteRange")]
        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> DeleteRangeAsync(IEnumerable<int> ids)
        {
            return CreateActionResult(await _categoryService.DeleteRangeAsync(ids));
        }


        [HttpPost("AddRange")]
        public async Task<IActionResult> AddRangeAsync(IEnumerable<CategoryCreateDto> dtos)
        {
            return CreateActionResult(await _categoryService.AddRangeAsync(dtos));
        }
    }
}
