using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Repositories
{
    public class OrderRepository: Repository<Order>
    {
        public OrderRepository(ApplicationContext db) : base(db)
        {
        }

    }
}
