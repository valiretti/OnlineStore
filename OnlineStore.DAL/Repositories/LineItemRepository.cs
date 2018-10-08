using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Repositories
{
    public class LineItemRepository : Repository<LineItem>
    {
        public LineItemRepository(ApplicationContext db) : base(db)
        {
        }

        public override IEnumerable<LineItem> GetAll()
        {
            return db.LineItems.Include(t => t.Phone).Include(t => t.Order);
        }


        public override IQueryable<LineItem> Find(Expression<Func<LineItem, bool>> predicate)
        {
            return db.LineItems.Include(t => t.Phone).Include(t=>t.Order).Where(predicate);
        }

        }
}
