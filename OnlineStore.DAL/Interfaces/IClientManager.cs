using System;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);

        ClientProfile GetClientProfile(string id);
        void Update(ClientProfile item);
    }
}
