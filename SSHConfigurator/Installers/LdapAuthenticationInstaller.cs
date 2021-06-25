using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSHConfigurator.Data;
using SSHConfigurator.Identity;
using SSHConfigurator.Models;
using Microsoft.EntityFrameworkCore;
using SSHConfigurator.Options;

namespace SSHConfigurator.Installers
{
    /// <summary>
    /// This class configures the services related to the LDAP authentication.
    /// </summary>
    public class LdapAuthenticationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            // LDAP Directory & Authentication settings --------------------------------------------------------------------------------------------------------------
            services.Configure<LdapSettings>(configuration.GetSection("LdapSettings"));
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("testdb"));
            services.AddIdentity<THUMember, IdentityRole>()
            .AddUserManager<LdapUserManager>()
            .AddSignInManager<LdapSignInManager>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
                        
        }
    }
}
