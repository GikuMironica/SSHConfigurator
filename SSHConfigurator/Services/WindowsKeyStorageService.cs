using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public class WindowsKeyStorageService : IKeyStorageService
    {
        public Task<bool> IsUserExistent(string Username)
        {
            return Task.FromResult(false);
        }

        public Task<bool> StorePublicKey(string Keyname, string Username)
        {
            throw new NotImplementedException();
        }
    }
}
