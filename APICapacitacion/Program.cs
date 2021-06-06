using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICapacitacion.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace APICapacitacion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((env, config) => {
                     //TODO: 1- configuracion del ambiente
                     var ambiente = env.HostingEnvironment.EnvironmentName;
                     config.AddJsonFile($"appsettings.{ambiente}.json", optional: true, reloadOnChange: true);
                     config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                     config.AddEnvironmentVariables();
                     if (args != null)
                     {
                         config.AddCommandLine(args);
                     }
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
