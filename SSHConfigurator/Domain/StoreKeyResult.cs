using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Domain
{
    public class StoreKeyResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
