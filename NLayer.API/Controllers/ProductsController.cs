using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{

    public class ProductsController : CustomBaseController
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductsController( IMapper mapper,IProductService productService)
        {
            _mapper = mapper;
            _service = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _service.GetAllAsync();
            var productDtos = _mapper.Map<List<ProductDto>>(products.ToList());

            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productDtos));
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productDto));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ProductCreateDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var createdProductDto = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, createdProductDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ProductUpdateDto productDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(product);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("DeleteRange")]
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> DeleteRangeAsync(IEnumerable<int> ids)
        {
            var products = await _service.Where(x => ids.Contains(x.Id)).ToListAsync();
            await _service.DeleteRangeAsync(products);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        [HttpPost("AddRange")]
        public async Task<IActionResult> AddRangeAsync(IEnumerable<ProductCreateDto> dtos)
        {
            IEnumerable<Product> products = await _service.AddRangeAsync(_mapper.Map<IEnumerable<Product>>(dtos));
            var createdProductDtos = _mapper.Map<IEnumerable<ProductCreateDto>>(products);

            return CreateActionResult(CustomResponseDto<IEnumerable<ProductCreateDto>>.Success(201, createdProductDtos));
        }


    }
}
