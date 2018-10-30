using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;


namespace OnlineStore.DAL.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(ApplicationContext db) : base(db)
        {
        }

        public override IQueryable<Product> GetAll()
        {
            return db.Products.Include(t=>t.Company).Include(t=>t.Category);
        }

        public override IQueryable<Product> Find(Expression<Func<Product, bool> >predicate)
        {
            return db.Products.Include(t=> t.Company).Include(t => t.Category).Where(predicate);
        }
       
    }
}
