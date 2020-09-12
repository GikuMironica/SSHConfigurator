using SSHConfigurator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    /// <summary>
    /// This service class implements the logic for manipulating the user's public key for Windows operating systems.
    /// TODO: The powershell scripts are not implemented.
    ///       The methods are not implemented.
    /// </summary>
    public class WindowsKeyStorageService : IKeyStorageService
    {


        /// <summary>
        /// This method deletes the user's public key from the target machine if exists.
        /// </summary>
        public Task DeletePublicKeyAsync(string Username)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// This method checks whether the user has already uploaded a public key.
        /// </summary>
        public Task<bool> HasKeyAsync(string Username)
        {
            return Task.FromResult(false);
        }


        /// <summary>
        /// This method stores the public key in the appropriate location on the target machine.
        /// </summary>
        public Task<StoreKeyResult> StorePublicKeyAsync(string Keyname, string Username)
        {
            throw new NotImplementedException();
        }
    }
}
