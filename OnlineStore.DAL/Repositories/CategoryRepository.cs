using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Repositories
{
    public class CategoryRepository: Repository<Category>
    {
        public CategoryRepository(ApplicationContext db) : base(db) { }
    }
}
