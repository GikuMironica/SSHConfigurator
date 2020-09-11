using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.ViewModels
{
    public class HomeViewModel
    {
        public string UserName { get; set; }
        public bool HasKey { get; set; }
        public string Token { get; set; }
    }
}
