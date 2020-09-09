using Microsoft.Extensions.Options;
using SSHConfigurator.Domain;
using SSHConfigurator.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    public class LinuxKeyStorageService : IKeyStorageService
    {
        private readonly KeyStorageScripts _ShellScripts;
        private readonly SystemAdmin _admin;

        public LinuxKeyStorageService(IOptions<KeyStorageScripts> scripts, IOptions<SystemAdmin> admin)
        {
            _ShellScripts = scripts.Value;
            _admin = admin.Value;
        }


        public async Task<bool> HasKey(string Username)
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
            await process.StandardInput.WriteLineAsync(string.Format(_ShellScripts.CheckUserAndKeyScript, Username));
            process.StandardInput.Close();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadLineAsync();
            process.WaitForExit();
            process.Close();

            if (!string.IsNullOrEmpty(error)) { return false ; }
            if (output.Contains("0"))
                return false;

            return true;
        }

        public async Task<StoreKeyResult> StorePublicKey(string Keyname, string Username)
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
            await process.StandardInput.WriteLineAsync(string.Format(_ShellScripts.StoreKeyScript, Username, Keyname, _admin.AdminUsername));
            process.StandardInput.Close();
            string output = await process.StandardOutput.ReadLineAsync();
            string error = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();
            process.Close();

            if (!string.IsNullOrEmpty(error))
            {
                return new StoreKeyResult
                {
                    IsSuccessful = false,
                    ErrorMessage = error
                };
            }
            if (output.Contains("0"))
                return new StoreKeyResult
                {
                    IsSuccessful = false,
                    ErrorMessage = "Something went wrong"
                };

            return new StoreKeyResult
            {
                IsSuccessful = true
            };
        }
               
    }
}
