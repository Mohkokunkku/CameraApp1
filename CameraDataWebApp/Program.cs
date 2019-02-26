using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CameraDataWebApp.WordProcessing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CameraDataWebApp
{
    public class Program
    {
        public static ConfigurationBuilder config;
        
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            //config = new ConfigurationBuilder().AddCommandLine(args).Build();
            //var hosturl = conf
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .UseKestrel()
            .UseUrls(@"http://192.168.100.210:47850");
    }
}
