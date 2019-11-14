using Microsoft.EntityFrameworkCore;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parking.API.Context;

namespace Parking.API.Services
{
    public class CarService
    {
        public readonly ParkingContext db;

        public CarService(ParkingContext db)
        {
            this.db = db;
        }

        public async Task<bool> RegisterNewCar(Car car)
        {
            var m = db.Set<Model>().Where(x => x.Id == car.Model.Id).FirstOrDefault();

            var t = db.Set<Core.Models.Type>().Where(x => x.Id == car.Type.Id).FirstOrDefault();

            if (m == null || t == null)
            {
                return false;
            }

            car.Folder = await AddressFolderByIdAsync(car);

            db.Cars.Add(car);

            await db.SaveChangesAsync();

            return true;

        }

        public async Task<bool> RegisterNewModel(Model model)
        {

            var m = db.Set<Manufacturer>().Where(x => x.Id == model.Manufacturer.Id).FirstOrDefault();

            if (m == null)
            {
                return false;
            }

            db.Models.Add(model);
            await db.SaveChangesAsync();

            return true;

        }

        public async Task<bool> RegisterNewType(Core.Models.Type type)
        {
            var name = type.Name;
            var m = db.Set<Core.Models.Type>().Where(x => x.Name == name).FirstOrDefault();

            if (m == null)
            {
                db.Types.Add(type);
                await db.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<List<string>> AddressFolderAsync()
        {
            var m = db.Cars.Select(x => x.Folder).ToList();
            return m;
        }


        public async Task<string> AddressFolderByIdAsync(Car car)
        {          

            StringBuilder str = new StringBuilder();
            
            var address = str.AppendFormat("{0}__{1}__{2}__{3}", car.Model.Manufacturer.Name, car.Model.Name, car.Model.Year, car.Type.Name).ToString();
            str.Clear();

            return address;
        }

    }
}
