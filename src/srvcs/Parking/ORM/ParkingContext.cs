using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Parking.API.Context
{
    public class ParkingContext : DbContext
    {
        const string schema = "Parking";

        public DbSet<Car> Cars { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<ParkedCar> Parked { get; set; }

        public DbSet<Slot> Slots { get; set; }

        public DbSet<SlotType> SlotTypes { get; set; }

        public DbSet<Core.Models.Type> Types { get; set; }

        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
            if (!Cars.Any())
            {
                InitializeDatabase();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ParkingDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SlotType>().HasKey(x => new { x.SlotId, x.TypeId });
            modelBuilder.HasDefaultSchema(schema);
        }

        internal static void ConfigureDBContext(SqlServerDbContextOptionsBuilder obj)
        {
            obj.MigrationsHistoryTable("__Migrations", schema);
        }

        public void InitializeDatabase()
        {
            var envDir = Environment.CurrentDirectory;

            string projectDirectory = Directory.GetParent(envDir).Parent.FullName;

            string path = projectDirectory + @"/shrd/Core/Dataset/labelsFile.txt";
            path = path.Replace(@"\", "/");

            if (System.IO.File.Exists(path))
            {
                Console.WriteLine($"Populando database...");

                // Busca as labels e diretórios o arquivo
                string line;
                var carList = new List<string>();

                using (var file = new System.IO.StreamReader(path))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        // monta o carro

                        var label = line.Split(',')[0];
                        var directory = line.Split(',')[1];

                        string[] separator = { "__" };

                        var names = label.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        var manufacturerName = names[0];
                        var modelName = names[1];
                        var year = names[2];
                        var typeName = names[3];

                        var manufacturer = Manufacturers.FirstOrDefault(x => x.Name.Equals(manufacturerName));

                        if (manufacturer == null)
                        {
                            manufacturer = new Manufacturer
                            {
                                Name = manufacturerName,
                                CreateAt = DateTime.Now
                            };

                            // Salva no banco
                            Manufacturers.Add(manufacturer);
                            SaveChanges();

                            Console.WriteLine($"Fabricante {manufacturer.Name} adicionado ao banco [{manufacturer.Id}]");
                        }

                        var model = Models.FirstOrDefault(x => x.Name.Equals(modelName) && x.Year.ToString().Equals(year));

                        if (model == null)
                        {
                            model = new Model
                            {
                                Manufacturer = manufacturer,
                                Name = modelName,
                                CreateAt = DateTime.Now,
                                Year = Convert.ToInt32(year)
                            };

                            // Salva no banco
                            Models.Add(model);
                            SaveChanges();

                            Console.WriteLine($"Modelo {model.Name} adicionado ao banco [{model.Id}]");
                        }

                        var type = Types.FirstOrDefault(x => x.Name.ToLower().Equals(typeName.ToLower()));

                        if (type == null)
                        {
                            type = new Core.Models.Type
                            {
                                CreateAt = DateTime.Now,
                                Name = typeName
                            };

                            Types.Add(type);
                            SaveChanges();

                            Console.WriteLine($"Tipo {type.Name} adicionado ao banco [{type.Id}]");
                        }

                        var car = Cars.FirstOrDefault(x => x.Model.Name.Equals(modelName));

                        if (car == null)
                        {
                            car = new Car
                            {
                                Model = model,
                                CreateAt = DateTime.Now,
                                Type = type,
                                Folder = directory
                            };

                            // Salva no banco
                            Cars.Add(car);
                            SaveChanges();

                            Console.WriteLine($"Carro {car.Model.Name} adicionado ao banco [{car.Id}]");
                        }
                    }

                    Console.WriteLine($"Database preenchida.");
                }
            }
        }

    }
    public class ParkingContextDesignTimeFactory : IDesignTimeDbContextFactory<ParkingContext>
    {
        public ParkingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ParkingContext>();
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ParkingDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", ParkingContext.ConfigureDBContext);

            return new ParkingContext(optionsBuilder.Options);
        }
    }
}
