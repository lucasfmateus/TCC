using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.API.Context
{
    public static class ORMExtensions
    {
        //Realiza migration e update data base ao rodar projeto
        public static IWebHost MigrateDatabase<T>(this IWebHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<T>();
                context.Database.Migrate();
            }

            return webHost;
        }
    }
}
