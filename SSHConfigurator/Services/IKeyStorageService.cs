﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public interface IKeyStorageService
    {
        public Task<bool> IsUserExistent(string Username);

        public void StorePublicKey(string KeyPath);
    }
}
