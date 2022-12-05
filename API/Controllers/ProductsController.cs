using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
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
    public ProductsController(StoreContext context)
    {
      _context = context;

    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Products()
    {
      var products = await _context.Products.ToListAsync();
      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Product(int id)
    {
      if (id <= 0)
        return BadRequest("Id can not be zero or less then zero");

      var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
      return Ok(product);
    }
  }
}