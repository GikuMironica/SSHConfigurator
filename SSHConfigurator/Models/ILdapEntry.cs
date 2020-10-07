using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Models
{
    public interface ILdapEntry
    {
        string ObjectSid { get; set; }
        string ObjectGuid { get; set; }
        string ObjectCategory { get; set; }
        string ObjectClass { get; set; }
        string Name { get; set; }
        string CommonName { get; set; }
        string DistinguishedName { get; set; }
    }
}
