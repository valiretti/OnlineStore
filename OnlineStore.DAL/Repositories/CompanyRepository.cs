using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Repositories
{
    public class CompanyRepository : Repository<Company>
    {
        public CompanyRepository(ApplicationContext db) : base(db) { }


    }
}
