using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SSHConfigurator.Installers
{
    /// <summary>
    /// An interface for classes meant to configure services and the app's request pipeline.
    /// </summary>
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
