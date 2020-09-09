using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace SSHConfigurator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "wwwroot", "scripts", "linux", "BashScripts.json"), optional: false, reloadOnChange: true);
                    config.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "wwwroot", "scripts", "windows", "PowerShellScripts.json"), optional: false, reloadOnChange: true);
                })
                 .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                 })
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
