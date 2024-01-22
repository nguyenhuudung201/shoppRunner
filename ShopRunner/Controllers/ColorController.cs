using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopRunner.DTOs;
using ShopRunner.Entities;
using ShopRunner.Repositories;


namespace ShopRunner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IGenericRepository<Color> _colorRepository;
        private readonly IGenericRepository<Product> _productRepository;
        public ColorController(IGenericRepository<Color> sizeRepository, IGenericRepository<Product> productRepository)
        {
            _colorRepository = sizeRepository;
            _productRepository = productRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var colors = await _colorRepository.GetRangeAsync();
            var colorToReturn =new List<ColorGetDto>(colors.Count());
            foreach (var c in colors)
            {
                colorToReturn.Add(new ColorGetDto
                {
                    Id = c.Id,
                    ColorName = c.Name,
                    NumbersOfProducts = await _productRepository.CountAsync(p => p.ColorId == c.Id)
                });
            }
            return Ok(colorToReturn);
        }
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetcolorById([FromRoute] int id)
        {
            var color = await _colorRepository.GetAsync(c => c.Id == id);
            if (color is null)
                return NotFound();
            var colorToReturn = new ColorGetAdminDto { Id = color.Id, Name = color.Name };
            return Ok(colorToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ColorCreateDto dto)
        {
            var color = new Color
            {
                Name = dto.Name,
            };
            await _colorRepository.AddAsync(color);
            await _colorRepository.SaveAsync();
            var c = await _colorRepository.GetAsync(c => c.Id == color.Id);
            var colorToReturn = new ColorGetAdminDto
            {
                Id = c!.Id,
                Name = c.Name,
            };
            return Created("", colorToReturn);
        }
        [HttpPut("{id}")]

        public async Task<IActionResult> Update([FromForm] ColorUpdateDto dto, [FromRoute] int id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color is null) return NotFound();
            color.Name = dto.Name;           
            _colorRepository.Update(color);
            await _colorRepository.SaveAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color is null) return NotFound();
            _colorRepository.Remove(color);
            await _colorRepository.SaveAsync();
            return NoContent();
        }
    }
    }

