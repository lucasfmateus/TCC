using Core.Models;
using Parking.API.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Classification.Services
{
    public class ClassificationService
    {
        public readonly ParkingContext db;

        public ClassificationService(ParkingContext db)
        {
            this.db = db;
        }

        public async Task<Car> ClassificationResultAsync(string folder)
        {
            return new Car
            {
                Id = "77",
                CreateAt = DateTimeOffset.Now,
                Model = new Model
                {
                    Name = "Fusquinha",
                    Id = "77",
                    CreateAt = DateTimeOffset.Now,
                    Year = 2019,
                    Manufacturer = new Manufacturer
                    {
                        CreateAt = DateTime.Now,
                        Id = "77",
                        Name = "Volks"
                    },

                },
                Type = new Core.Models.Type
                {
                    Id = "77",
                    CreateAt = DateTimeOffset.Now,
                    Name = "Sport"
                }
            };
        } 
    }
}
