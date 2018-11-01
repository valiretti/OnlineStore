using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Repositories
{
    public class LineItemRepository : Repository<LineItem>
    {
        public LineItemRepository(ApplicationContext db) : base(db)
        {
        }

        public override IQueryable<LineItem> GetAll()
        {
            return db.LineItems.Include(t => t.Product).Include(t => t.Order);
        }


        public override IQueryable<LineItem> Find(Expression<Func<LineItem, bool>> predicate)
        {
            return db.LineItems.Include(t => t.Product).Include(t=>t.Order).Where(predicate);
        }

        }
}
