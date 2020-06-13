using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Models
{
    public class THUMember : IdentityUser
    {        
        public Int64 LifetimeEnd { get; set; }

        [NotMapped]
        public string ObjectSid { get; set; }

        [NotMapped]
        public string ObjectGuid { get; set; }

        [NotMapped]
        public string ObjectCategory { get; set; }

        [NotMapped]
        public string ObjectClass { get; set; }

        [NotMapped]
        public string Password { get; set; }

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
