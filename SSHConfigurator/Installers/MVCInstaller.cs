using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SSHConfigurator.Installers
{
    /// <summary>
    /// This class configures the services related to the MVC framework.
    /// </summary>
    public class MVCInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            
            // Configure MVC, require user to be logged in to use app
            services.AddControllersWithViews(options => {
                var policy = new AuthorizationPolicyBuilder()                                              
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }
    }
}
