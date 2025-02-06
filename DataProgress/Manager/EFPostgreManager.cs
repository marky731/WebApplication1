using Intermediary.DbContext;
using Microsoft.EntityFrameworkCore;


public class EFPostgreManager
{
    private readonly ApplicationDbContext _context;

    public EFPostgreManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> LoadData<T>(string sql) where T : class
    {
        return _context.Set<T>().FromSqlRaw(sql).ToList();
    }

    public T? LoadDataSingle<T>(string sql) where T : class
    {
        return _context.Set<T>().FromSqlRaw(sql).FirstOrDefault();
    }

    public bool ExecuteSql(string sql)
    {
        return _context.Database.ExecuteSqlRaw(sql) > 0;
    }

    public int ExecuteSqlWithRowCount(string sql)
    {
        return _context.Database.ExecuteSqlRaw(sql);
    }
}