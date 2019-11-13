using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.API.Context;
using Parking.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.API.Controller
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public readonly ParkingContext db;
        public readonly CarService service;

        public CarController(ParkingContext db)
        {
            this.db = db;
            this.service = new CarService(db);
        }

        [Route("GetCars/")]
        [HttpGet]
        public List<Car> GetAllCar()
        {
            var p = db.Cars.Include(x => x.Model)
                           .Include(x => x.Model.Manufacturer)
                           .Include(x => x.Type)
                           .ToList();
            return p;
        }


        [Route("GetAdress/")]
        [HttpGet]
        public async Task<List<string>> GetAddressFolder()
        {
            return await service.AddressFolderAsync();
        }

        [Route("GetCarById/")]
        [HttpGet]
        public Car GetCarById([FromQuery]string id)
        {
            var p = db.Cars.Include(x => x.Model)
               .Include(x => x.Type)
               .Where(x => x.Id == id)
               .FirstOrDefault();

            return p;
        }

        [Route("GetModels/")]
        [HttpGet]
        public List<Model> GetAllModels()
        {
            var p = db.Models.Include(x => x.Manufacturer)
                             .ToList();
            return p;
        }

        [Route("GetModelByName")]
        [HttpGet]
        public Model GetModelsByName([FromQuery]string name)
        {
            var p = db.Models.Include(x => x.Manufacturer)
                             .Where(x => x.Name == name)
                             .FirstOrDefault();
            return p;
        }

        [Route("GetManufactures/")]
        [HttpGet]
        public List<Manufacturer> GetAllManufactures()
        {
            var p = db.Manufacturers.ToList();
            return p;
        }

        [Route("GetTypes/")]
        [HttpGet]
        public List<Core.Models.Type> GetAllTypes()
        {
            var p = db.Types.ToList();
            return p;
        }

        [Route("GetManufacturesByName")]
        [HttpGet]
        public Manufacturer GetManufacturesByName([FromQuery] string name)
        {
            var p = db.Manufacturers.Where(x => x.Name == name).FirstOrDefault();
            return p;
        }

        [Route("GetTypeByName")]
        [HttpGet]
        public Core.Models.Type GetTypeByName([FromQuery] string name)
        {
            var p = db.Types.Where(x => x.Name == name).FirstOrDefault();
            return p;
        }

        [Route("GetModelByNameAsync")]
        [HttpGet]
        public Model GetModelById([FromQuery] string name)
        {
            var p = db.Models.Where(x => x.Name == name).FirstOrDefault();
            return p;
        }

        [Route("NewManufacture/")]
        [HttpPost]
        public async Task<bool> AsyncAddManufacture([FromBody] Manufacturer manufacturer)
        {
            try
            {
                db.Manufacturers.Add(manufacturer);
                await db.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        [Route("NewModel/")]
        [HttpPost]
        public async Task<bool> AsyncAddModel([FromBody] Model model)
        {
            var x = await service.RegisterNewModel(model);
            if (x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [Route("NewCar/")]
        [HttpPost]
        public async Task<bool> AsyncAddCar([FromBody] Car car)
        {
            var x = await service.RegisterNewCar(car);
            if (x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Route("NewCarType/")]
        [HttpPost]
        public async Task<bool> AsyncAddType([FromBody] Core.Models.Type type)
        {
            var x = await service.RegisterNewType(type);
            if (x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
