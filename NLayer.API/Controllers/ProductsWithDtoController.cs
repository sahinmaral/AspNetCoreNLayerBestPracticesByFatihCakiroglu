using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

using System.Collections.Generic;

namespace NLayer.API.Controllers
{
    
    public class ProductsWithDtoController : CustomBaseController
    {
        private readonly IProductServiceWithDto _service;

        public ProductsWithDtoController(IProductServiceWithDto productService)
        {
            _service = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return CreateActionResult(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return CreateActionResult(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ProductCreateDto productDto)
        {
            return CreateActionResult(await _service.AddAsync(productDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ProductUpdateDto productDto)
        {
            return CreateActionResult(await _service.UpdateAsync(productDto));
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return CreateActionResult(await _service.DeleteAsync(id));
        }

        [HttpDelete("DeleteRange")]
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> DeleteRangeAsync(IEnumerable<int> ids)
        {
            return CreateActionResult(await _service.DeleteRangeAsync(ids));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        [HttpPost("AddRange")]
        public async Task<IActionResult> AddRangeAsync(IEnumerable<ProductCreateDto> dtos)
        {
            return CreateActionResult(await _service.AddRangeAsync(dtos));
        }

    }
}
