using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class ProductRepository : IProductRepository
  {
    private readonly StoreContext _context;
    public ProductRepository(StoreContext context)
    {
      _context = context;

    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
    {
      var productBrands = await _context.ProductBrands.ToListAsync();
      return productBrands;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
      var product = await _context.Products
                       .Include(x => x.ProductBrand)
                       .Include(x => x.ProductType)
                       .FirstOrDefaultAsync(x => x.Id == id);


      //var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);


      return product;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {

      /* var typeId = 1;

       var productsType = _context.Products
                          .Where(x => x.ProductTypeId == typeId)
                          .Include(x => x.ProductType);

       var types = await productsType.ToListAsync();*/



      var products = await _context.Products
                         .Include(x => x.ProductBrand)
                         .Include(x => x.ProductType)
                         .ToListAsync();

      // var products = await _context.Products.ToListAsync();


      return products;
    }

    public async Task<IReadOnlyList<ProductType>> GetProductTypes()
    {
      var productTypes = await _context.ProductTypes.ToListAsync();
      return productTypes;
    }
  }
}