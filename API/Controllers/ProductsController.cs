using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly StoreContext _context;
    private readonly IProductRepository _repo;
    public ProductsController(StoreContext context, IProductRepository repo)
    {
      _repo = repo;
      _context = context;

    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> Products()
    {
      var products = await _repo.GetProductsAsync();
      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Product(int id)
    {
      if (id <= 0)
        return BadRequest("Id can not be zero or less then zero");

      var product = await _repo.GetProductByIdAsync(id);
      return Ok(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrandsAsync()
    {
      var productBrands = await _repo.GetProductBrandsAsync();
      return Ok(productBrands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
      var productTypes = await _repo.GetProductTypes();
      return Ok(productTypes);
    }
  }
}