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
                db.Categories.Add(type);
                await db.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<List<string>> AddressFolderAsync()
        {
            var m = db.Cars.Include(x => x.Model).Select(x => x.Model.Name).ToList();
            var p = db.Cars.Include(x => x.Model).Select(x => x.Model.Manufacturer.Name).ToList();
            var t = db.Cars.Include(x => x.Type).Select(x => x.Type.Name).ToList();

            List<string> address = new List<string>();

            StringBuilder str = new StringBuilder();

            for (int i = 0; i < m.Count; i++)
            {
                address.Add(str.AppendFormat("{0}__{1}__{2}", p[i], m[i], t[i]).ToString());
                str.Clear();
            }

            return address;
        }

    }
}
