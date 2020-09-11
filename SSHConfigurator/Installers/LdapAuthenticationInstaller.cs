using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSHConfigurator.Data;
using SSHConfigurator.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using SSHConfigurator.Models;
using Microsoft.EntityFrameworkCore;
using SSHConfigurator.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Threading.Tasks;

namespace SSHConfigurator.Installers
{
    /// <summary>
    /// This class configures the services related to the LDAP authentication, authorization and the app's request pipeline.
    /// </summary>
    public class LdapAuthenticationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            // LDAP-Authentication configurations
            services.Configure<LdapSettings>(configuration.GetSection("LdapSettings"));
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("testdb"));
            services.AddIdentity<THUMember, IdentityRole>()
            .AddUserManager<LdapUserManager>()
            .AddSignInManager<LdapSignInManager>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            // Allowed Courses Configuration
            services.Configure<AllowedCourses>(configuration.GetSection("Courses"));
        }
    }
}
