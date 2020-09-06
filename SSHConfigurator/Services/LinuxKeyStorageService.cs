using Microsoft.Extensions.Options;
using SSHConfigurator.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public class LinuxKeyStorageService : IKeyStorageService
    {
        private readonly KeyStorageScripts _ShellScripts;

        public LinuxKeyStorageService(IOptions<KeyStorageScripts> scripts)
        {
            _ShellScripts = scripts.Value;
        }


        public bool IsUserExistent(string Username)
        {            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {                    
                    FileName = string.Format(_ShellScripts.CheckUserScript, Username),                    
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error)) { return false ; }
            if (output.Contains("1"))
                return true;

            return false;
        }

        public void StorePublicKey(string KeyPath)
        {
            throw new NotImplementedException();
        }
    }
}
