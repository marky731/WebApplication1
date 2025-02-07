using Intermediary.DbContext;
using Microsoft.EntityFrameworkCore;
using DbContext = Intermediary.DbContext.DbContext;


public class EFPostgreManager
{
    private readonly DbContext _context;

    public EFPostgreManager(DbContext context)
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