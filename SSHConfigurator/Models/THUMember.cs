using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Models
{
    public class THUMember : IdentityUser
    {        
        public Int64 LifetimeEnd { get; set; }
    }
}
