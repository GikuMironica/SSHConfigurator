using SSHConfigurator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public class WindowsKeyStorageService : IKeyStorageService
    {
        public async Task DeletePublicKeyAsync(string Username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasKeyAsync(string Username)
        {
            return Task.FromResult(false);
        }

        public Task<StoreKeyResult> StorePublicKeyAsync(string Keyname, string Username)
        {
            throw new NotImplementedException();
        }
    }
}
