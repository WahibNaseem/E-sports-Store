using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
  public class StoreContextSeed
  {
    private static string brandsUrl = "../Infrastructure/Data/SeedData/brands.json";
    private static string typesUrl = "../Infrastructure/Data/SeedData/types.json";
    private static string productsUrl = "../Infrastructure/Data/SeedData/products.json";

    public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
    {
      try
      {
        if (!context.ProductBrands.Any())
        {
          var brandData = File.ReadAllText(brandsUrl);

          var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

          await context.ProductBrands.AddRangeAsync(brands);

          await context.SaveChangesAsync();
        }

        if (!context.ProductTypes.Any())
        {
          var productTypeData = File.ReadAllText(typesUrl);

          var types = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);

          await context.AddRangeAsync(types);
          await context.SaveChangesAsync();
        }

        if (!context.Products.Any())
        {
          var productsData = File.ReadAllText(productsUrl);

          var products = JsonSerializer.Deserialize<List<Product>>(productsData);

          await context.AddRangeAsync(products);
          await context.SaveChangesAsync();
        }
      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<StoreContextSeed>();

        logger.LogError(ex.Message);
      }

    }
  }
}