using System.Linq.Expressions;

namespace Tarea1_Workshop.Repoditory.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        Task add(T entity);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = false);
        Task Remove(T entity);
        Task Save();
        Task Update(T entity);
    }
}
