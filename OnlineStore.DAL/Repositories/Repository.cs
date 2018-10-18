using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using OnlineStore.DAL.EF;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public ApplicationContext db;

        public Repository(ApplicationContext db)
        {
            this.db = db;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return db.Set<T>();
        }

        public T Get(int id)
        {
            return db.Set<T>().Find(id);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return db.Set<T>().Where(predicate);
        }

        public void Create(T item)
        {
            db.Set<T>().Add(item);
        }

        public void Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            T t = db.Set<T>().Find(id);
            if (t != null)
            {
                db.Set<T>().Remove(t);
            }
        }
    }
}
