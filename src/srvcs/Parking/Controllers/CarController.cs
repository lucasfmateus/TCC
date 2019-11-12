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
        public async Task<ActionResult<Manufacturer>> AsyncAddManufacture([FromBody] Manufacturer manufacturer)
        {
            db.Manufacturers.Add(manufacturer);
            await db.SaveChangesAsync();
            return CreatedAtAction("Item Adicionado:", new { id = manufacturer.Id }, manufacturer);
        }

        [Route("NewModel/")]
        [HttpPost]
        public async Task<ActionResult<Model>> AsyncAddModel([FromBody] Model model)
        {
            var x = await service.RegisterNewModel(model);
            if (x)
            {
                return CreatedAtAction("Item Adicionado:", new { id = model.Id }, model);
            }
            else
            {
                return CreatedAtAction("Falha ao adicionar Model:", new { id = model.Id }, model);
            }
        }


        [Route("NewCar/")]
        [HttpPost]
        public async Task<ActionResult<Car>> AsyncAddCar([FromBody] Car car)
        {
            var x = await service.RegisterNewCar(car);
            if (x)
            {
                return CreatedAtAction("Item Adicionado:", new { id = car.Id }, car);
            }
            else
            {
                return CreatedAtAction("Falha ao adicionar Car:", new { id = car.Id }, car);
            }
        }

        [Route("NewCarCategory/")]
        [HttpPost]
        public async Task<ActionResult<Core.Models.Type>> AsyncAddType([FromBody] Core.Models.Type type)
        {
            var x = await service.RegisterNewType(type);
            if (x)
            {
                return CreatedAtAction("Item Adicionado:", new { id = type.Id }, type);
            }
            else
            {
                return CreatedAtAction("Falha ao adicionar Model:", new { id = type.Id }, type);
            }
        }
    }
}
