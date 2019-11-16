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
            //Inicializa projeto antes e feito a migration
            CreateHostBuilder(args).Build().MigrateDatabase<ParkingContext>().Run();
        }

        //Declara roda em que API se comunica, e inicializa o Projeto parking onde se encontra todos os metodos e comunicacao com o banco da API
        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5001")
                .UseStartup<ParkingApiService>();
    }
}
