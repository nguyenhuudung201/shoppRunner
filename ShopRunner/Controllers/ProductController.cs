using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ShopRunner.DTOs;
using ShopRunner.Entities;
using ShopRunner.Migrations;
using ShopRunner.Models;
using ShopRunner.Repositories;

namespace ShopRunner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductController(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll([FromQuery] RequestParamsForProduct? requestParams)
        {
            var filter = PredicateBuilder.New<Product>(true);
            
            if (requestParams?.Price is not null)
            {
                if (int.TryParse(requestParams.Price.Split("-")[0], out var priceFrom) && int.TryParse(requestParams.Price.Split("-")[1], out var priceTo))
                {
                    filter = filter.And(p => p.Price >= priceFrom && p.Price <= priceTo);
                }            
            }
            if (requestParams?.SizeId is not null && requestParams.SizeId > 0)
            {
                filter = filter.And(p => p.SizeId == requestParams.SizeId);
            }
            if (requestParams?.ColorId is not null && requestParams.ColorId > 0)
            {
                filter = filter.And(p => p.ColorId == requestParams.ColorId);
            }
            if (requestParams?.CategoryId is not null && requestParams.CategoryId > 0)
            {
                filter = filter.And(p => p.CategoryId == requestParams.CategoryId);
            }
            var products = await _productRepository.GetRangeAsync(filter, new List<string> { "Color", "Category", "Size" });

            var productsToReturn = products.Select(p => new ProductGetDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Color = p.Color!.Name,
                ColorId= p.ColorId,
                Size = p.Size!.Name,
                SizeId=p.SizeId,
                Category = p.Category!.Name,
                CategoryId=p.CategoryId
            });

            return Ok(productsToReturn);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var product = await _productRepository.GetAsync(p => p.Id == id, new List<string> { "Color", "Size", "Category" });
            if (product is null)
                return NotFound();
            var productToReturn = new ProductGetDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                Color = product.Color!.Name,
                ColorId= product.ColorId,
                Size = product.Size!.Name,
                SizeId=product.SizeId,
                Category = product.Category!.Name,
                CategoryId=product.CategoryId
                
            };
            return Ok(productToReturn);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            FileInfo fileInfo = new FileInfo(dto.Imgage.FileName);
            string fileName = "product_image_" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                dto.Imgage.CopyTo(stream);
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = $"/images/{fileName}",
                Price = dto.Price,
                ColorId = dto.ColorId,
                SizeId = dto.SizeId,
                CategoryId = dto.CategoryId,
            };
            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();
            var p = await _productRepository.GetAsync(p => p.Id == product.Id, new List<string> { "Color", "Size", "Category" });
            var productToReturn = new ProductGetDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Color = p.Color!.Name,
                ColorId=p.ColorId,
                Size = p.Size!.Name,
                SizeId=p.SizeId,
                Category = p.Category!.Name,
                CategoryId=p.CategoryId,
            };
            return Created("", productToReturn);
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Update( [FromForm] ProductUpdateDto dto, [FromRoute] int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return NotFound();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{product.Image}");
            System.IO.File.Delete(filePath);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            FileInfo fileInfo = new FileInfo(dto.Image.FileName);

            string fileName = "product_image_" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                dto.Image.CopyTo(stream);
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.ColorId = dto.ColorId;
            product.SizeId = dto.SizeId;
            product.CategoryId = dto.CategoryId;
            product.Image = $"/images/{fileName}";
            _productRepository.Update(product);
            await _productRepository.SaveAsync();

            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete ([FromRoute] int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return NotFound();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{product.Image}");
            System.IO.File.Delete(filePath);

            _productRepository.Remove(product);
            await _productRepository.SaveAsync();
            return NoContent();
        }
    }

}
