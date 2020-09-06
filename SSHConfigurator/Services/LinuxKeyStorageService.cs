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


        public async Task<bool> IsUserExistent(string Username)
        {            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {                    
                    FileName = "bash",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true
                }
            };
            process.Start();
            await process.StandardInput.WriteLineAsync(string.Format(_ShellScripts.CheckUserScript, Username));
            process.StandardInput.Close();
            string output = await process.StandardOutput.ReadLineAsync();
            string error = await process.StandardError.ReadLineAsync();
            process.WaitForExit();
            process.Close();

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
