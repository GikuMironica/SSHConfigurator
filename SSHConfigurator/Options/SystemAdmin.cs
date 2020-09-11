using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Options
{
    /// <summary>
    /// This is a configuration class containing the Admin's username on the target machine.
    /// Can be injected with Dependency Injection in the required services.
    /// </summary>
    public class SystemAdmin
    {
        public string AdminUsername { get; set; }
    }
}
