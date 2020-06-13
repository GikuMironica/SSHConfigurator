using Novell.Directory.Ldap;
using SSHConfigurator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public class LdapService : ILdapSerivce
    {
        public LdapService()
        {

        }

        public bool Authenticate(string distinguishedName, string password)
        {
            using (var ldapConnection = new LdapConnection() { SecureSocketLayer = true })
            {
                ldapConnection.Connect(this._ldapSettings.ServerName, this._ldapSettings.ServerPort);

                try
                {
                    ldapConnection.Bind(distinguishedName, password);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public THUMember GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public ICollection<THUMember> GetUsersInGroup(string group)
        {
            return this.GetUsersInGroups(this.GetGroups(group));
        }

        public ICollection<THUMember> GetUsersInGroups(ICollection<Models.LdapEntry> groups = null)
        {
            var users = new Collection<THUMember>();

            if (groups == null || !groups.Any())
            {
                users.AddRange(this.GetChildren<LdapUser>(this._searchBase));
            }
            else
            {
                foreach (var group in groups)
                {
                    users.AddRange(this.GetChildren<LdapUser>(this._searchBase, @group.DistinguishedName));
                }
            }

            return users;
        }

        public ICollection<Models.LdapEntry> GetGroups(string groupName, bool getChildGroups = false)
        {
            var groups = new Collection<Models.LdapEntry>();

            var filter = $"(&(objectClass=group)(cn={groupName}))";

            using (var ldapConnection = this.GetConnection())
            {
                var search = ldapConnection.Search(
                    this._searchBase,
                    LdapConnection.SCOPE_SUB,
                    filter,
                    this._attributes,
                    false,
                    null,
                    null);

                LdapMessage message;

                while ((message = search.getResponse()) != null)
                {
                    if (!(message is LdapSearchResult searchResultMessage))
                    {
                        continue;
                    }

                    var entry = searchResultMessage.Entry;

                    groups.Add(this.CreateEntryFromAttributes(entry.DN, entry.getAttributeSet()));

                    if (!getChildGroups)
                    {
                        continue;
                    }

                    foreach (var child in this.GetChildren<Identity.Models.LdapEntry>(string.Empty, entry.DN))
                    {
                        groups.Add(child);
                    }
                }
            }

            return groups;
        }

        private ILdapConnection GetConnection()
        {
            var ldapConnection = new LdapConnection() { SecureSocketLayer = this._ldapSettings.UseSSL };

            //Connect function will create a socket connection to the server - Port 389 for insecure and 3269 for secure    
            ldapConnection.Connect(this._ldapSettings.ServerName, this._ldapSettings.ServerPort);
            //Bind function with null user dn and password value will perform anonymous bind to LDAP server 
            ldapConnection.Bind(this._ldapSettings.Credentials.DomainUserName, this._ldapSettings.Credentials.Password);

            return ldapConnection;
        }
    }
}
