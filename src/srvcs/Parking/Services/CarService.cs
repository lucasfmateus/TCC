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
            try
            {
                var m = db.Set<Model>().Where(x => x.Id == car.Model.Id).FirstOrDefault();

                var t = db.Set<Core.Models.Type>().Where(x => x.Id == car.Type.Id).FirstOrDefault();

                var c = db.Set<Car>().Where(x => x.Id == car.Id).FirstOrDefault();

                if (m == null || t == null)
                {
                    return false;
                }

                if (c != null)
                {
                    if (car.Model != null)
                    {
                        c.Model = car.Model;
                    }

                    if (car.Type != null)
                    {
                        c.Type = car.Type;
                    }
                }
                else
                {
                    db.Cars.Add(car);
                }

                car.Folder = await AddressFolderByIdAsync(car);

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }

        public async Task<bool> RegisterOrUpdateNewModel(Model model)
        {
            try
            {
                var m = db.Set<Manufacturer>().Where(x => x.Id == model.Manufacturer.Id).FirstOrDefault();

                var p = db.Set<Model>().Where(x => x.Id == model.Id).FirstOrDefault();

                if (m == null)
                {
                    return false;
                }

                if (p != null)
                {
                    p = model;
                }
                else
                {
                    db.Models.Add(model);
                }

                await db.SaveChangesAsync();

                return true;
            }
            catch(Exception) 
            {
                return false;
            }
            
        }

        public async Task<bool> RegisterOrUpdateNewManufacturer(Manufacturer manufacturer)
        {
            
            var m = db.Set<Manufacturer>().Where(x => x.Id == manufacturer.Id).FirstOrDefault();

            if(m == null)
            {
                db.Manufacturers.Add(manufacturer);

            }
            else
            {
                m.Name = manufacturer.Name;
            }

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterOrUpdateNewType(Core.Models.Type type)
        {
            try
            {
                var m = db.Set<Core.Models.Type>().Where(x => x.Id == type.Id).FirstOrDefault();

                if (m == null)
                {
                    db.Types.Add(type);
                }
                else
                {
                    if (type.Name != null)
                    {
                        m.Name = type.Name;
                    }
                }

                await db.SaveChangesAsync();

                return true;

            }
            catch (Exception)
            {
                return false;
            }

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
