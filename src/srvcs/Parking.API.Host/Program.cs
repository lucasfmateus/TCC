using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Parking.API;
using Parking.API.Context;

namespace ParkingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Inicializa projeto antes e faz a migration inicial
            CreateHostBuilder(args).Build().MigrateDatabase<ParkingContext>().Run();
        }

        //Inicializa o Projeto parking onde se encontram todos os metodos e comunicação com o banco da API
        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5002")
                .UseStartup<ParkingApiService>();
    }
}
