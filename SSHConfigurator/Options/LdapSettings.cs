using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Options
{
    public class LdapSettings
    {
        public string ServerName { get; set; }

        public int ServerPort { get; set; }

        public bool UseSSL { get; set; }

        public string SearchBase { get; set; }

        public string ContainerName { get; set; }

        public string DomainName { get; set; }

        public string DomainDistinguishedName { get; set; }
    }
    
}
