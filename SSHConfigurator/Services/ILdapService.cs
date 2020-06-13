using SSHConfigurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public interface ILdapService
    {
        ICollection<THUMember> GetUsersInGroup(string groupName);

        ICollection<THUMember> GetUsersInGroups(ICollection<LdapEntry> groups = null);

        THUMember GetUserByUserName(string userName);

        bool Authenticate(string distinguishedName, string password);
    }
}
