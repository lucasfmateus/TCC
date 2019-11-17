using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Parking.API.Context;
using Parking.API.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Parking.API
{
    public class ParkingApiService
    {
        public IConfiguration Configuration { get; }

        public ParkingApiService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("Policy", builder => {
                builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
            }));

            services.AddMvc();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddEntityFrameworkSqlServer();

            //declaracao do contexto
            services.AddDbContext<ParkingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Configura o Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Parking API", Version = "v1" });
            });

            //injecao de dependencia dos services
            services.AddSingleton<CarService>();
            services.AddSingleton<ParkedService>();

        }

        public void Configure(IApplicationBuilder app)
        {            
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking API");
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
