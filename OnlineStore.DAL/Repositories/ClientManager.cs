using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public ApplicationContext Database { get; set; }
        public ClientManager(ApplicationContext db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }

        public ClientProfile GetClientProfile(string id)
        {
            return Database.ClientProfiles.Find(id);
        }

        public IQueryable<ClientProfile> Find(Expression<Func<ClientProfile, bool>> predicate)
        {
            return Database.ClientProfiles.Where(predicate);
        }

        public void Update(ClientProfile item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
