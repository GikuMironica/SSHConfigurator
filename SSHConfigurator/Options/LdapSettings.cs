﻿using System;
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

        public LdapCredentials Credentials { get; set; }
    }

    public class LdapCredentials
    {
        public string DomainUserName { get; set; }

        public string Password { get; set; }
    }
}
