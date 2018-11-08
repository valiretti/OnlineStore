using System;
using System.Linq;
using System.Linq.Expressions;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);

        ClientProfile GetClientProfile(string id);
        IQueryable<ClientProfile> Find(Expression<Func<ClientProfile, bool>> predicate);
        void Update(ClientProfile item);
    }
}
