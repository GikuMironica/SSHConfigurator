using SSHConfigurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public interface ILdapService
    {
        THUMember GetUserByUserName(string userName, string password);
    }
}
