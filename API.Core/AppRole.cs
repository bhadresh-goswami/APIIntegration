using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core
{
    public class AppRole:IdentityRole
    {
    }
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(IRoleStore<AppRole> store, IEnumerable<IRoleValidator<AppRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<AppRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
