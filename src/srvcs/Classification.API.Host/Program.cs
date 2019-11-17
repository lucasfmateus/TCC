using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Parking.API.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classification.API.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5003")
                .UseStartup<ClassificationApiService>();        
    }
}
