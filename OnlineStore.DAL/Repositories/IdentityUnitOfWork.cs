﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineStore.DAL.EF;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Identity;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Repositories
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private ApplicationContext db;
        private OrderRepository orderRepository;
        private PhoneRepository phoneRepository;
        private CompanyRepository companyRepository;
        private LineItemRepository lineItemRepository;


        public IdentityUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            ClientManager = new ClientManager(db);
        }

        public ApplicationUserManager UserManager { get; }

        public IClientManager ClientManager { get; }

        public ApplicationRoleManager RoleManager { get; }

        public IRepository<Phone> Phones
        {
            get
            {
                if (phoneRepository == null)
                    phoneRepository = new PhoneRepository(db);
                return phoneRepository;
            }
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
            }

        }

        public IRepository<Company> Companies
        {
            get
            {
                if (companyRepository == null)
                    companyRepository = new CompanyRepository(db);
                return companyRepository;
            }
        }

        public IRepository<LineItem> LineItems
        {
            get
            {
                if (lineItemRepository == null)
                    lineItemRepository = new LineItemRepository(db);
                return lineItemRepository;
            }
        }
        
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
                    RoleManager.Dispose();
                    ClientManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
