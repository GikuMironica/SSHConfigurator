using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSHConfigurator.Installers;
using System;
using System.Linq;

namespace SSHConfigurator.Extensions
{
    public static class InstallerExtensions
    {
        // Extension method for IServiceCollection 
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            // enumerate all classes implementing IInstaller, instantiate them
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface &&
                !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            // call the installServices method on each class that inherits from IInstaller interface
            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
