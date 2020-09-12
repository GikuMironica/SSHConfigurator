using Microsoft.Extensions.Logging;
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
    /// <summary>
    /// This service class implements the logic for manipulating the user's public key for Linux operating systems.
    /// </summary>
    public class LinuxKeyStorageService : IKeyStorageService
    {
        private readonly KeyStorageScripts _ShellScripts;
        private readonly SystemConfiguration _systemConfig;
        private readonly ILogger<LinuxKeyStorageService> _logger;

        public LinuxKeyStorageService(IOptions<KeyStorageScripts> scripts, IOptions<SystemConfiguration> systemConfig, ILogger<LinuxKeyStorageService> logger)
        {
            _ShellScripts = scripts.Value;
            _systemConfig = systemConfig.Value;
            _logger = logger;
        }

        
        /// <summary>
        /// This method checks whether the user has already uploaded a public key.
        /// </summary>
        public async Task<bool> HasKeyAsync(string Username)
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

            if (!string.IsNullOrEmpty(error)) 
            {
                //_logger.LogError(error);
                return false ; 
            }
            if (output.Contains("0"))
                return false;

            return true;
        }

        /// <summary>
        /// This method stores the public key in the appropriate location on the target machine.
        /// </summary>
        public async Task<StoreKeyResult> StorePublicKeyAsync(string Keyname, string Username)
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
            await process.StandardInput.WriteLineAsync(string.Format(_ShellScripts.StoreKeyScript, Username, Keyname, _systemConfig.AdminUsername, _systemConfig.AccountLifeTime));
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
                    ErrorMessage = error + "\n "+ string.Format(_ShellScripts.StoreKeyScript, Username, Keyname, _systemConfig.AdminUsername, _systemConfig.AccountLifeTime)
                };
            }
            if (!string.IsNullOrEmpty(output))
            {
                if (output.Contains("0"))
                    return new StoreKeyResult
                    {
                        IsSuccessful = false,
                        ErrorMessage = "Something went wrong"
                    };
            }
            return new StoreKeyResult
            {
                IsSuccessful = true
            };
        }


        /// <summary>
        /// This method deletes the user's public key from the target machine if exists.
        /// </summary>
        public async Task DeletePublicKeyAsync(string Username)
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
            await process.StandardInput.WriteLineAsync(string.Format(_ShellScripts.DeleteKeyScript, Username));
            process.StandardInput.Close();
            process.WaitForExit();
            process.Close();
        }
    }
}
