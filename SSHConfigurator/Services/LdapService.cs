using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using SSHConfigurator.Extensions;
using SSHConfigurator.Models;
using SSHConfigurator.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public class LdapService : ILdapService
    {
        private readonly LdapSettings _ldapSettings;

        private readonly string[] _attributes =
        {
            "objectSid", "objectGUID", "objectCategory", "objectClass", "memberOf", "name", "cn", "distinguishedName",
            "sAMAccountName", "sAMAccountName", "userPrincipalName", "displayName", "givenName", "sn", "description",
            "telephoneNumber", "mail", "streetAddress", "postalCode", "l", "st", "co", "c"
        };

        public LdapService(IOptions<LdapSettings> ldapSettings)
        {
            this._ldapSettings = ldapSettings.Value;
        }

        private ILdapConnection GetConnection()
        {
            var ldapConnection = new LdapConnection() { SecureSocketLayer = this._ldapSettings.UseSSL };

            // Create a socket connection to the server
            ldapConnection.Connect(this._ldapSettings.ServerName, this._ldapSettings.ServerPort);
            // bind anonymously
            ldapConnection.Bind(_ldapSettings.Credentials.DomainUserName, _ldapSettings.Credentials.Password);
            return ldapConnection;
        }


        public bool Authenticate(string distinguishedName, string password)
        {
            return true;
        }

        public THUMember GetUserByUserName(string userName)
        {
            THUMember user = null;

            var filter = $"(&(cn={userName}))";

            using (var ldapConnection = this.GetConnection())
            {
               // var searchUser = this._ldapSettings.SearchBase.Replace("Users", userName);
                var search = ldapConnection.Search(
                    this._ldapSettings.SearchBase,
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

                    user = this.CreateUserFromAttributes(this._ldapSettings.SearchBase, searchResultMessage.Entry.getAttributeSet());
                }
            }

            return user;
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
                users.AddRange(this.GetChildren<THUMember>(this._ldapSettings.SearchBase));
            }
            else
            {
                foreach (var group in groups)
                {
                    users.AddRange(this.GetChildren<THUMember>(this._ldapSettings.SearchBase, @group.DistinguishedName));
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
                    this._ldapSettings.SearchBase,
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

                    foreach (var child in this.GetChildren<Models.LdapEntry>(string.Empty, entry.DN))
                    {
                        groups.Add(child);
                    }
                }
            }

            return groups;
        }

        
        private THUMember CreateUserFromAttributes(string distinguishedName, LdapAttributeSet attributeSet)
        {
            var ldapUser = new THUMember();

            ldapUser.ObjectSid = attributeSet.getAttribute("objectSid")?.StringValue;
            ldapUser.ObjectGuid = attributeSet.getAttribute("objectGUID")?.StringValue;
            ldapUser.ObjectCategory = attributeSet.getAttribute("objectCategory")?.StringValue;
            ldapUser.ObjectClass = attributeSet.getAttribute("objectClass")?.StringValue;
            try
            {
                ldapUser.MemberOf = attributeSet.getAttribute("memberOf").StringValueArray.ToList();
            }
            catch(NullReferenceException e)
            {

            }
            ldapUser.CommonName = attributeSet.getAttribute("cn")?.StringValue;
            ldapUser.UserName = attributeSet.getAttribute("name")?.StringValue;
            ldapUser.Name = attributeSet.getAttribute("name")?.StringValue;
            ldapUser.DistinguishedName = attributeSet.getAttribute("distinguishedName")?.StringValue ?? distinguishedName;
            ldapUser.DisplayName = attributeSet.getAttribute("displayName")?.StringValue;
            ldapUser.Email = attributeSet.getAttribute("mail")?.StringValue;
             
                        
            return ldapUser;
        }

        private Models.LdapEntry CreateEntryFromAttributes(string distinguishedName, LdapAttributeSet attributeSet)
        {
            return new Models.LdapEntry
            {
                ObjectSid = attributeSet.getAttribute("objectSid")?.StringValue,
                ObjectGuid = attributeSet.getAttribute("objectGUID")?.StringValue,
                ObjectCategory = attributeSet.getAttribute("objectCategory")?.StringValue,
                ObjectClass = attributeSet.getAttribute("objectClass")?.StringValue,
                CommonName = attributeSet.getAttribute("cn")?.StringValue,
                Name = attributeSet.getAttribute("name")?.StringValue,
                DistinguishedName = attributeSet.getAttribute("distinguishedName")?.StringValue ?? distinguishedName,
                SamAccountName = attributeSet.getAttribute("sAMAccountName")?.StringValue,
                SamAccountType = int.Parse(attributeSet.getAttribute("sAMAccountType")?.StringValue ?? "0"),
            };
        }


        private ICollection<ILdapEntry> GetChildren(string searchBase, string groupDistinguishedName = null,
            string objectCategory = "*", string objectClass = "*")
        {
            var allChildren = new Collection<ILdapEntry>();

            var filter = string.IsNullOrEmpty(groupDistinguishedName)
                ? $"(&(objectCategory={objectCategory})(objectClass={objectClass}))"
                : $"(&(objectCategory={objectCategory})(objectClass={objectClass})(memberOf={groupDistinguishedName}))";

            using (var ldapConnection = this.GetConnection())
            {
                var search = ldapConnection.Search(
                    searchBase,
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

                    if (objectClass == "group")
                    {
                        allChildren.Add(this.CreateEntryFromAttributes(entry.DN, entry.getAttributeSet()));

                        foreach (var child in this.GetChildren(string.Empty, entry.DN, objectCategory, objectClass))
                        {
                            allChildren.Add(child);
                        }
                    }

                    if (objectClass == "user")
                    {
                        allChildren.Add(this.CreateUserFromAttributes(entry.DN, entry.getAttributeSet()));
                    }

                    ;
                }
            }
            return allChildren;
        }

        private ICollection<T> GetChildren<T>(string searchBase, string groupDistinguishedName = null)
            where T : ILdapEntry, new()
        {
            var entries = new Collection<T>();

            var objectCategory = "*";
            var objectClass = "*";

            if (typeof(T) == typeof(Models.LdapEntry))
            {
                objectClass = "group";
                objectCategory = "group";

                entries = this.GetChildren(this._ldapSettings.SearchBase, groupDistinguishedName, objectCategory, objectClass)
                    .Cast<T>().ToCollection();

            }

            if (typeof(T) == typeof(THUMember))
            {
                objectCategory = "person";
                objectClass = "user";

                entries = this.GetChildren(this._ldapSettings.SearchBase, null, objectCategory, objectClass).Cast<T>()
                    .ToCollection();

            }

            return entries;
        }
    }
}
