using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public class WindowsKeyStorageService : IKeyStorageService
    {
        public bool IsUserExistent(string Username)
        {
            return false;
        }

        public void StorePublicKey(string KeyPath)
        {
            throw new NotImplementedException();
        }
    }
}
