﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSHConfigurator.Data;
using SSHConfigurator.Models;
using SSHConfigurator.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Installers
{
    public class LdapAuthenticationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<LdapSettings>(configuration.GetSection("LdapSettings"));

            services.AddDbContext<DataContext>();
            services.AddIdentity<THUMember, IdentityRole>(options =>
            {
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        }
    }
}
