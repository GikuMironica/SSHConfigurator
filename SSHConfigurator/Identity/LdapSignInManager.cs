using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SSHConfigurator.Models;
using SSHConfigurator.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Identity
{
    /// <summary>
    /// This class overrides the basic functionality of the SignInManager<T> from Identity Framework
    /// in order to work along with LDAP authentication & authorization.
    /// </summary>
    public class LdapSignInManager : SignInManager<THUMember>
    {
        private readonly IOptions<AllowedCourses> coursesOptions;
        private readonly LdapUserManager ldapUserManager;

        public LdapSignInManager(LdapUserManager ldapUserManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<THUMember> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor, ILogger<LdapSignInManager> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<THUMember> confirmation,
            IOptions<AllowedCourses> coursesOptions) 
            : base(ldapUserManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            this.coursesOptions = coursesOptions;
            this.ldapUserManager = ldapUserManager;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockOutOnFailure)
        {
            var user = await ldapUserManager.FindByNameAsync(userName , password);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            var validUser = false;
            if (coursesOptions.Value.CourseNames != null)
            {
                foreach (var course in coursesOptions.Value.CourseNames)
                {
                    if (user.MemberOf.FirstOrDefault(s => s.Contains(course)) != null)
                        validUser = true;
                }
                if (!validUser)
                {
                    return SignInResult.NotAllowed;
                }
            }
            return await this.PasswordSignInAsync(user, password, rememberMe, lockOutOnFailure);
        }
    }
}
