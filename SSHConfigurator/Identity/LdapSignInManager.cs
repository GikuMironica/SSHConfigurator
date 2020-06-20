using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SSHConfigurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Identity
{
    public class LdapSignInManager : SignInManager<THUMember>
    {
        public LdapSignInManager(LdapUserManager ldapUserManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<THUMember> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor, ILogger<LdapSignInManager> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<THUMember> confirmation) 
            : base(ldapUserManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {

        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockOutOnFailure)
        {
            var user = await this.UserManager.FindByNameAsync(userName);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await this.PasswordSignInAsync(user, password, rememberMe, lockOutOnFailure);
        }
    }
}
