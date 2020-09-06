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

        public void StorePublicKey(string KeyPath)
        {
            throw new NotImplementedException();
        }
    }
}
