using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class PostgreManagerEF
    {
        private readonly ApplicationDbContext _context;

        public PostgreManagerEF(ApplicationDbContext context)
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
}