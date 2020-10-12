using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SSHConfigurator.Models;
using SSHConfigurator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Identity
{
    /// <summary>
    /// This class overrides the basic functionality of the UserManager<T> from Identity Framework
    /// in order to work along with LDAP authentication & authorization.
    /// </summary>
    public class LdapUserManager : UserManager<THUMember>
    {
        private readonly ILdapService ldapService;

        public LdapUserManager(ILdapService ldapService, IUserStore<THUMember> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<THUMember> passwordHasher, IEnumerable<IUserValidator<THUMember>> userValidators,
            IEnumerable<IPasswordValidator<THUMember>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<LdapUserManager> logger) 
            : base (store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
                keyNormalizer, errors, services, logger)
        {
            this.ldapService = ldapService;
        }

        public override async Task<bool> CheckPasswordAsync(THUMember user, string password)
        {
            return await Task.FromResult(true);
        }

        public override Task<bool> HasPasswordAsync(THUMember user)
        {
            return Task.FromResult(true);
        }

        public Task<THUMember> FindByNameAsync(string userName, string password)
        {
            return Task.FromResult(this.ldapService.GetUserByUserName(userName, password));
        }

        /*public override Task<THUMember> FindByNameAsync(string userName)
       {
           return Task.FromResult(this.ldapService.GetUserByUserName(userName, null));
       }*/
    }
}

