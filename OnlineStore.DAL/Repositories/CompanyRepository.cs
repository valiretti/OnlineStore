using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Repositories
{
    public class CompanyRepository : Repository<Company>
    {
        public CompanyRepository(ApplicationContext db) : base(db) { }


    }
}
