using API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Interface
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserById(string userId);
        Task<AppUser> GetUserByUsername(string username);
        Task<AppUser> CreateUser(AppUser user, string password);
    }
}
