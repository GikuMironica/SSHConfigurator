using SSHConfigurator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public class WindowsKeyStorageService : IKeyStorageService
    {
        public Task<bool> HasKey(string Username)
        {
            return Task.FromResult(false);
        }

        public Task<StoreKeyResult> StorePublicKey(string Keyname, string Username)
        {
            throw new NotImplementedException();
        }
    }
}
