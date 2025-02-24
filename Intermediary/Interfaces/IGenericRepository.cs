namespace Intermediary.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    Task<List<T>> GetAllAsync();
    Task<int> GetTotalCountAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}