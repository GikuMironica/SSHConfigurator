using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using SSHConfigurator.Models;
using SSHConfigurator.Options;
using System.Linq;

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
            var ldapUser = new THUMember
            {
                ObjectSid = attributeSet.getAttribute("objectSid")?.StringValue,
                ObjectGuid = attributeSet.getAttribute("objectGUID")?.StringValue,
                ObjectCategory = attributeSet.getAttribute("objectCategory")?.StringValue,
                ObjectClass = attributeSet.getAttribute("objectClass")?.StringValue,
                MemberOf = attributeSet.getAttribute("memberOf")?.StringValueArray.ToList(),
                CommonName = attributeSet.getAttribute("cn")?.StringValue,
                UserName = attributeSet.getAttribute("name")?.StringValue,
                Name = attributeSet.getAttribute("name")?.StringValue,
                DistinguishedName = attributeSet.getAttribute("distinguishedName")?.StringValue ?? distinguishedName,
                DisplayName = attributeSet.getAttribute("displayName")?.StringValue,
                Email = attributeSet.getAttribute("mail")?.StringValue
            };


            return ldapUser;
        }

       
    }
}
