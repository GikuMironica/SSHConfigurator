using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Options
{
    public class KeyStorageScripts
    {
        public String CheckUserAndKeyScript { get; set; }
        public String CreateUserScript { get; set; }
        public String StoreKeyScript { get; set; }
        public String DeleteKeyScript { get; set; }
    }
}
