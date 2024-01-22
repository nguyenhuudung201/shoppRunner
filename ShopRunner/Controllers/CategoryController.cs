using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopRunner.DTOs;
using ShopRunner.Entities;
using ShopRunner.Migrations;
using ShopRunner.Repositories;

namespace ShopRunner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryController(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetRangeAsync();
            var categoryToReturn = new List<CategoryGetDto>(categories.Count());
            foreach (var c in categories)
            {
                categoryToReturn.Add(new CategoryGetDto
                {
                    CategoryId = c.Id,
                    Name = c.Name,
                    Image=c.Image
                });
            }
            return Ok(categoryToReturn);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == id);
            if (category is null)
                return NotFound();
            var categoryToReturn= new CategoryGetDto { CategoryId = category.Id,Name=category.Name,Image=category.Image };
            return Ok(categoryToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto dto)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            FileInfo fileInfo = new FileInfo(dto.Image.FileName);
            string fileName = "category_image_" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                dto.Image.CopyTo(stream);
            }

            var category = new Category
            {
                Name = dto.Name,
               
                Image = $"/images/{fileName}",
               
            };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
            var c = await _categoryRepository.GetAsync(c => c.Id == category.Id);
            var categoryToReturn = new CategoryGetDto
            {
                CategoryId = c.Id,
                Name = c.Name,
               
                Image = c.Image,
               
            };
            return Created("", categoryToReturn);
        }
        [HttpPut("{id}")]
    
        public async Task<IActionResult> Update([FromForm] CategoryUpdateDto dto, [FromRoute] int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return NotFound();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{category.Image}");
            System.IO.File.Delete(filePath);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            FileInfo fileInfo = new FileInfo(dto.Image.FileName);

            string fileName = "category_image_" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                dto.Image.CopyTo(stream);
            }

            category.Name = dto.Name;
            category.Image = $"/images/{fileName}";
            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();


            return NoContent();
        }
        [HttpDelete("{id}")]
     
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return NotFound();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{category.Image}");
            System.IO.File.Delete(filePath);

            _categoryRepository.Remove(category);
            await _categoryRepository.SaveAsync();
            return NoContent();
        }
    }
}
