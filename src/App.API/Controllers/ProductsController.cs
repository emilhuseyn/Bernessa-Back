using App.Business.DTOs.Products;
using App.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(new { success = true, data = products });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(new { success = true, data = product });
        }

        [HttpGet("{id}/related")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRelatedProducts(int id, [FromQuery] int limit = 8)
        {
            if (limit < 1 || limit > 50)
            {
                return BadRequest(new { success = false, message = "Limit 1-50 arasında olmalıdır" });
            }

            var products = await _productService.GetRelatedProductsAsync(id, limit);
            return Ok(new { success = true, data = products });
        }

        [HttpGet("featured")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeatured()
        {
            var products = await _productService.GetFeaturedProductsAsync();
            return Ok(new { success = true, data = products });
        }

        [HttpGet("category/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(string slug)
        {
            var products = await _productService.GetProductsByCategorySlugAsync(slug);
            return Ok(new { success = true, data = products });
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            {
                return BadRequest(new { success = false, message = "Axtarış ən azı 2 simvol olmalıdır" });
            }

            if (q.Length > 100)
            {
                return BadRequest(new { success = false, message = "Axtarış maksimum 100 simvol ola bilər" });
            }

            var products = await _productService.SearchProductsAsync(q);
            return Ok(new { success = true, data = products });
        }

        [HttpGet("deals")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDeals()
        {
            var products = await _productService.GetDealsAsync();
            return Ok(new { success = true, data = products });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromForm] CreateProductDTO createProductDto)
        {
            var product = await _productService.CreateProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, new { success = true, data = product, message = "Məhsul uğurla yaradıldı" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateProductDTO updateProductDto)
        {
            var product = await _productService.UpdateProductAsync(id, updateProductDto);
            return Ok(new { success = true, data = product, message = "Məhsul uğurla yeniləndi" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok(new { success = true, message = "Məhsul uğurla silindi" });
        }
    }
}