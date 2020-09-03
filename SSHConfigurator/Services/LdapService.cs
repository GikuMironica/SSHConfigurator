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
        private readonly string _usernamePrefix = "hs-ulm\\";

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

        private ILdapConnection GetConnection(string userName, string password)
        {
            var ldapConnection = new LdapConnection() { SecureSocketLayer = this._ldapSettings.UseSSL };

            // Create a socket connection to the server
            ldapConnection.Connect(this._ldapSettings.ServerName, this._ldapSettings.ServerPort);
            // bind 
            try
            {
                ldapConnection.Bind(_usernamePrefix + userName, password);
            }
            catch(LdapException)
            {
                return null;
            }
            return ldapConnection;
        }


        public THUMember GetUserByUserName(string userName, string password)
        {
            THUMember user = null;

            var filter = $"(&(cn={userName}))";

            using (var ldapConnection = this.GetConnection(userName, password))
            {
                if (ldapConnection == null)
                    return null;
                
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
             


        private THUMember CreateUserFromAttributes(string distinguishedName, LdapAttributeSet attributeSet)
        {
            var ldapUser = new THUMember();

            ldapUser.ObjectSid = attributeSet.getAttribute("objectSid")?.StringValue;
            ldapUser.ObjectGuid = attributeSet.getAttribute("objectGUID")?.StringValue;
            ldapUser.ObjectCategory = attributeSet.getAttribute("objectCategory")?.StringValue;
            ldapUser.ObjectClass = attributeSet.getAttribute("objectClass")?.StringValue;
            ldapUser.MemberOf = attributeSet.getAttribute("memberOf")?.StringValueArray.ToList();
            ldapUser.CommonName = attributeSet.getAttribute("cn")?.StringValue;
            ldapUser.UserName = attributeSet.getAttribute("name")?.StringValue;
            ldapUser.Name = attributeSet.getAttribute("name")?.StringValue;
            ldapUser.DistinguishedName = attributeSet.getAttribute("distinguishedName")?.StringValue ?? distinguishedName;
            ldapUser.DisplayName = attributeSet.getAttribute("displayName")?.StringValue;
            ldapUser.Email = attributeSet.getAttribute("mail")?.StringValue;

            return ldapUser;
        }

       
    }
}
