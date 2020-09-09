using SSHConfigurator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public interface IKeyStorageService
    {
        public Task<bool> HasKeyAsync(string Username);

        public Task<StoreKeyResult> StorePublicKeyAsync(string Keyname, string Username);

        public Task DeletePublicKeyAsync(string Username);
    }
}
