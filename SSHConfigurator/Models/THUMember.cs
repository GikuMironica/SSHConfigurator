using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSHConfigurator.Models
{
    /// <summary>
    /// This class serves as a model for the entity that's going to be retrieved from the LDAP server upon authentication.
    /// </summary>
    public class THUMember : IdentityUser, ILdapEntry
    {        
        [NotMapped]
        public string ObjectSid { get; set; }

        [NotMapped]
        public string ObjectGuid { get; set; }

        [NotMapped]
        public string ObjectCategory { get; set; }

        [NotMapped]
        public string ObjectClass { get; set; }

        [NotMapped]
        public string Name { get; set; }

        [NotMapped]
        public string CommonName { get; set; }

        [NotMapped]
        public string DistinguishedName { get; set; }

        [NotMapped]
        public string DisplayName { get; set; }

        [NotMapped]
        public List<string> MemberOf { get; set; }

    }
}
