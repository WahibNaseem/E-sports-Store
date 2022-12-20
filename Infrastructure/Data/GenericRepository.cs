using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
  {
    private readonly StoreContext _context;
    public GenericRepository(StoreContext context)
    {
      _context = context;

    }
    public async Task<T> GetByIdAsync(int id)
    {
      var record = await _context.Set<T>().FindAsync(id);
      return record;
    }

    public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
      var records = await _context.Set<T>().ToListAsync();
      return records;
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).CountAsync();
    }


    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
      var appliedSpecs = SpecificationEvalutor<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
      return appliedSpecs;
    }
  }
}