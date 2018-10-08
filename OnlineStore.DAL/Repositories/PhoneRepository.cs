using System;
using System.Collections;
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
    public class PhoneRepository : Repository<Phone>
    {
        public PhoneRepository(ApplicationContext db) : base(db)
        {
        }
       public override IEnumerable<Phone> GetAll()
        {
            return db.Phones.Include(t=>t.Company);
        }

      
        public override IQueryable<Phone> Find(Expression<Func<Phone, bool> >predicate)
        {
            return db.Phones.Include(t=> t.Company).Where(predicate);
        }
       
    }
}
