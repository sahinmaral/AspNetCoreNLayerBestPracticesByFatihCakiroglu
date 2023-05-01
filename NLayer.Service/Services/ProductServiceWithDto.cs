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
    public class ProductServiceWithDto : ServiceWithDto<Product, ProductDto>, IProductServiceWithDto
    {
        private readonly IProductRepository _productRepository;

        public ProductServiceWithDto(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<CustomResponseDto<ProductCreateDto>> AddAsync(ProductCreateDto dto)
        {
            Product product = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();

            var newDto = _mapper.Map<ProductCreateDto>(product);
            return CustomResponseDto<ProductCreateDto>.Success(StatusCodes.Status200OK, newDto);
        }

        public async Task<CustomResponseDto<IEnumerable<ProductCreateDto>>> AddRangeAsync(IEnumerable<ProductCreateDto> dtos)
        {
            IEnumerable<Product> entities = _mapper.Map<IEnumerable<Product>>(dtos);
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();

            var newDtos = _mapper.Map<IEnumerable<ProductCreateDto>>(entities);
            return CustomResponseDto<IEnumerable<ProductCreateDto>>.Success(StatusCodes.Status200OK, newDtos);
        }

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = await _productRepository.GetProductsWithCategory();
            var productDtos = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productDtos);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            Product product = _mapper.Map<Product>(dto);

            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }
    }
}
