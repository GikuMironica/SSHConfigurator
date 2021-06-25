using SSHConfigurator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    /// <summary>
    /// Interface for the classes that are going to implement the logic for manipulating the user's public key depending on the target machine's operating system.
    /// The main purpose of this interface is for avoiding tight coupling. In the required services, this interface will be injected via Dependency Injection, 
    /// the implementation of this interface can be configured in the configuration files.
    /// </summary>
    public interface IKeyStorageService
    {
        public Task<bool> HasKeyAsync(string username);

        public Task<StoreKeyResult> StorePublicKeyAsync(string keyname, string username);

        public Task DeletePublicKeyAsync(string username);
    }
}
