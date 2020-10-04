using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace SSHConfigurator
{
    /// <summary>
    /// This is the entry point of the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// This is the first method that will be executed when the application starts
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("NLog.json", optional: true, reloadOnChange: true).Build();
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

            var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Shut down program because of error");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }


        /// <summary>
        /// This method fires up kestrel
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "wwwroot", "scripts", "linux", "BashScripts.json"), optional: false, reloadOnChange: true);
                    config.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "wwwroot", "scripts", "windows", "PowerShellScripts.json"), optional: false, reloadOnChange: true);
                })
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole();
                     logging.AddDebug();
                     logging.AddEventSourceLogger();
                     // Enable NLog as one of the Logging Provider
                     logging.AddNLog();
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
