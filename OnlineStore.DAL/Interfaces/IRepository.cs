using System;
using System.Linq;
using System.Linq.Expressions;

namespace OnlineStore.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(int id);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
      }
}
