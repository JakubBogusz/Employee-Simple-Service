using API.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _applicationDbContext;
    
    public GenericRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _applicationDbContext.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _applicationDbContext.Set<T>().FindAsync(id);
    }

    public void Add(T entity)
    {
        _applicationDbContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _applicationDbContext.Set<T>().Attach(entity);
        _applicationDbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _applicationDbContext.Set<T>().Remove(entity);
    }
}