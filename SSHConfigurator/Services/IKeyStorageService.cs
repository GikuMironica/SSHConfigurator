using SSHConfigurator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public interface IKeyStorageService
    {
        public Task<bool> HasKey(string Username);

        public Task<StoreKeyResult> StorePublicKey(string Keyname, string Username);
    }
}
