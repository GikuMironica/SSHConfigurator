using IdentityServer.LdapExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SSHConfigurator.Options;
using SSHConfigurator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SSHConfigurator.Installers
{
    /// <summary>
    /// This class configures the services custom developed for this solution, but not required for the MVC framework.
    /// </summary>
    public class ConfigurationsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // SERVICES
            services.AddTransient<ILdapService, LdapService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<GoogleRecaptchaService>();


            // CONFIGURATIONS
            // Configure the implementation for the public key storage depending on the OS.
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isLinux)
            {
                services.AddSingleton<IKeyStorageService, LinuxKeyStorageService>();
                services.Configure<KeyStorageScripts>(configuration.GetSection("ShellScripts"));
            }
            else
            {
                services.AddSingleton<IKeyStorageService, WindowsKeyStorageService>();
                services.Configure<KeyStorageScripts>(configuration.GetSection("PowerShellScripts"));
            }

            // configure the username of the admin
            // It is required for moving the uploaded public key by the students from wwwroot folder to the correct folder.
            services.Configure<SystemConfiguration>(configuration.GetSection("SystemConfiguration"));

            // configure google recaptcha credentials with Options Pattern
            services.Configure<RecaptchaSettings>(configuration.GetSection("GoogleReCaptcha"));
        }
    }
}
