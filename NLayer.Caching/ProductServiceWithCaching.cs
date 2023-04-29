using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;

using System.Linq.Expressions;


namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CACHE_PRODUCT_KEY = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;

            if(!memoryCache.TryGetValue(CACHE_PRODUCT_KEY,out _))
            {
                memoryCache.Set(CACHE_PRODUCT_KEY,_repository.GetProductsWithCategory().Result);
            }
        }


        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            await CacheAllProductsAsync();

            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();

            await CacheAllProductsAsync();

            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            return Task.FromResult(_memoryCache.Get<List<Product>>(CACHE_PRODUCT_KEY).Any(expression.Compile()));
        }

        public async Task DeleteAsync(Product entity)
        {
            _repository.Delete(entity);
            await _unitOfWork.CommitAsync();

            await CacheAllProductsAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Product> entities)
        {
            _repository.DeleteRange(entities);
            await _unitOfWork.CommitAsync();

            await CacheAllProductsAsync();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<List<Product>>(CACHE_PRODUCT_KEY).AsEnumerable());
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var products = _memoryCache.Get<List<Product>>(CACHE_PRODUCT_KEY);
            if(products == null)
            {
                throw new NotFoundException($"{typeof(Product).Name} not found with {id} id");
            }

            return Task.FromResult(products.FirstOrDefault(x => x.Id == id));
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = _memoryCache.Get<List<Product>>(CACHE_PRODUCT_KEY);
            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsWithCategoryDto));
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(CACHE_PRODUCT_KEY).Where(expression.Compile()).AsQueryable();
        }


        private async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(CACHE_PRODUCT_KEY, await _repository.GetAll().ToListAsync());
        }
    }
}