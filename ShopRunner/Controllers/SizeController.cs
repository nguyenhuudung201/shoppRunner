using Microsoft.AspNetCore.Mvc;
using ShopRunner.DatabaseContexts;
using ShopRunner.DTOs;
using ShopRunner.Entities;
using ShopRunner.Repositories;
using System.Data.Entity;

namespace ShopRunner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : Controller
    {
        private readonly IGenericRepository<Size> _sizeRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public SizeController (IGenericRepository<Size> sizeRepository, IGenericRepository<Product> productRepository)
        {
            _sizeRepository = sizeRepository;
            _productRepository = productRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var sizes = await _sizeRepository.GetRangeAsync();
            var sizesToReturn = new List<SizeGetDto>(sizes.Count());
            foreach (var s in sizes)
            {
                sizesToReturn.Add(new SizeGetDto
                {
                    SizeId = s.Id,
                    SizeName = s.Name,
                    NumbersOfProducts = await _productRepository.CountAsync(p => p.SizeId == s.Id)
                });
            }
            return Ok(sizesToReturn);
        }

    }
}
