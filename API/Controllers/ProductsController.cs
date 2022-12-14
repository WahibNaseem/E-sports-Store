using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
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
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Product> _prodRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    public ProductsController(StoreContext context, IProductRepository repo,
                              IGenericRepository<Product> prodRepo,
                              IGenericRepository<ProductBrand> productBrandRepo,
                               IGenericRepository<ProductType> productTypeRepo,
                               IMapper mapper)
    {
      _productBrandRepo = productBrandRepo;
      _prodRepo = prodRepo;
      _productTypeRepo = productTypeRepo;
      _repo = repo;
      _context = context;
      _mapper = mapper;

    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
    {
      // var products = await _repo.GetProductsAsync();
      // var products = await _prodRepo.ListAllAsync();

      var spec = new ProductsWithTypesAndBrandsSpecification();
      var products = await _prodRepo.ListAsync(spec);

      // var productsToReturnDto = products.Select(product => new ProductToReturnDto
      // {

      //   Id = product.Id,
      //   Name = product.Name,
      //   Description = product.Description,
      //   PictureUrl = product.PictureUrl,
      //   Price = product.Price,
      //   ProductBrand = product.ProductBrand.Name,
      //   ProductType = product.ProductType.Name

      // }).ToList();

      var productsToReturnDto = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
      return Ok(productsToReturnDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
      if (id <= 0)
        return BadRequest("Id can not be zero or less then zero");

      // var product = await _repo.GetProductByIdAsync(id);
      var spec = new ProductsWithTypesAndBrandsSpecification(id);
      var product = await _prodRepo.GetEntityWithSpec(spec);

      var productToReturnDto = _mapper.Map<Product, ProductToReturnDto>(product);
      return Ok(productToReturnDto);

    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrandsAsync()
    {
      // var productBrands = await _repo.GetProductBrandsAsync();
      var productBrands = await _productBrandRepo.ListAllAsync();

      return Ok(productBrands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
      // var productTypes = await _repo.GetProductTypes();
      var productTypes = await _productTypeRepo.ListAllAsync();
      return Ok(productTypes);
    }
  }
}