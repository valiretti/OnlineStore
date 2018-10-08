using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.BLL.Services
{
    public class UserService : IUserService
    {
        public UserService(IIdentityUnitOfWork dataBase)
        {
            DataBase = dataBase;
        }
        private IIdentityUnitOfWork DataBase { get; set; }

        public async Task<string> Create(UserDto userDto)
        {
            string result;
            ApplicationUser user = await DataBase.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                var create = await DataBase.UserManager.CreateAsync(user, userDto.Password);
                if (create.Errors.Any())
                {
                    result = "Registration Error";
                    return result;
                }
                await DataBase.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, Address = userDto.Address, Name = userDto.Name, Email = userDto.Email, PhoneNumber = userDto.PhoneNumber };
                DataBase.ClientManager.Create(clientProfile);
                await DataBase.SaveAsync();
                result = "OK";
                return result;
            }
            result = "User exists";
            return result;
        }

        public async Task<ClaimsIdentity> Authenticate(UserDto userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await DataBase.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
            {
                claim = await DataBase.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
                return claim;
            }
            return claim;
        }

        public async Task SetInitialData(UserDto adminDto, List<string> roles)
        {
            foreach (var roleName in roles)
            {
                var role = await DataBase.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await DataBase.RoleManager.CreateAsync(role);
                }

                await Create(adminDto);
            }
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
