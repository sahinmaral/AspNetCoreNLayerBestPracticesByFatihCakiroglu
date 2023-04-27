using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity, new()
    {
        // IQueryable ile verilerimiz uzerinde sorgulama islemleri yaptiktan sonra
        // ToList ve ToListAsync metodu ile veritabanindan verileri alir.
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
