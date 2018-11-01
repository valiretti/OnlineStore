using System.Linq;
using System.Security.Claims;
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

        public UserDto GetUserData(string id)
        {
            var user = DataBase.ClientManager.GetClientProfile(id);
            if (user != null)
            {
                return new UserDto()
                {
                    Name = user.Name,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email
                };
            }

            return null;
        }

        public void EditProfile(UserDto userDto, string userId)
        {
            var user = DataBase.UserManager.FindById(userId);
            if (user != null)
            {
                user.Email = userDto.Email;
                user.UserName = userDto.Email;
            }

            var result = DataBase.UserManager.Update(user);
            if (result.Succeeded)
            {
                var profile = DataBase.ClientManager.GetClientProfile(userId);

                profile.Name = userDto.Name;
                profile.Address = userDto.Address;
                profile.Email = userDto.Email;
                profile.PhoneNumber = userDto.PhoneNumber;

                DataBase.ClientManager.Update(profile);
                DataBase.Save();
            }
        }

        public void EditPassword(UserDto userDto, string userId)
        {
            var user = DataBase.UserManager.FindById(userId);
            if (user != null)
            {
                user.PasswordHash = DataBase.UserManager.PasswordHasher.HashPassword(userDto.Password);
                DataBase.UserManager.Update(user);
            }
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
