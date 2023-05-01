using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Web.Filters;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductAPIService _productAPIService;
        private readonly CategoryAPIService _categoryAPIService;

        public ProductController(ProductAPIService productAPIService, CategoryAPIService categoryAPIService)
        {
            _productAPIService = productAPIService;
            _categoryAPIService = categoryAPIService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productAPIService.GetProductsWithCategoryAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Save()
        {
            var categories = await _categoryAPIService.GetAllAsync();

            List<CategoryDto> categoryDtos = new List<CategoryDto>
            {
                new CategoryDto()
            {
                Name = "Choose one"
            }
            };
            categoryDtos.AddRange(categories);

            ViewBag.Categories = new SelectList(categoryDtos, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryAPIService.GetAllAsync();
                List<CategoryDto> categoryDtos = new List<CategoryDto>
            {
                new CategoryDto()
            {
                Name = "Choose one"
            }
            };
                categoryDtos.AddRange(categories);

                ViewBag.Categories = new SelectList(categoryDtos, "Id", "Name");

                return View(productDto);
            }

            await _productAPIService.SaveAsync(productDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var categories = await _categoryAPIService.GetAllAsync();

            var updatedProduct = await _productAPIService.GetByIdAsync(id);
            
            ViewBag.Categories = new SelectList(categories, "Id", "Name",updatedProduct.CategoryId);

            return View(updatedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if(!ModelState.IsValid)
            {
                var categories = await _categoryAPIService.GetAllAsync();

                var updatedProduct = await _productAPIService.GetByIdAsync(productDto.Id);

                ViewBag.Categories = new SelectList(categories, "Id", "Name", updatedProduct.CategoryId);

                return View(updatedProduct);
            }

            await _productAPIService.UpdateAsync(productDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productAPIService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
