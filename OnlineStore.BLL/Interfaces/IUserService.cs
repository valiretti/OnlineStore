using System;
using System.Security.Claims;
using System.Threading.Tasks;
using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<string> Create(UserDto userDto);
        Task<ClaimsIdentity> Authenticate(UserDto userDto);
        UserDto GetUserData(string id);
        void EditProfile(UserDto userDto, string userId);
        void EditPassword(UserDto userDto, string userId);
    }
}
