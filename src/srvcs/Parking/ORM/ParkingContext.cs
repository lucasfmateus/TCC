using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
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
