namespace API.Data.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync();
    
    Task<T> GetByIdAsync(int id);
    
    void Add(T entity);

    void Update(T entity);
        
    void Delete(T entity);
}