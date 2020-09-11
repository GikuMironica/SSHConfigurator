using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Options
{
    /// <summary>
    /// This is a configuration class containing the scripts for manipulating the users public key on the target machine.
    /// This class will load the PowerShell or Bash scripts depending on the target machine's operating system.
    /// It can be injected with Dependency Injection in the required view or service.
    /// </summary>
    public class KeyStorageScripts
    {
        public String CheckUserAndKeyScript { get; set; }
        public String StoreKeyScript { get; set; }
        public String DeleteKeyScript { get; set; }
    }
}
