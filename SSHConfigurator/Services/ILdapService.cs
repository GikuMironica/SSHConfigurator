using SSHConfigurator.Models;

namespace SSHConfigurator.Services
{
    public interface ILdapService
    {
        THUMember GetUserByUserName(string userName, string password);
    }
}
