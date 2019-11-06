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
    [Route("api/parked/")]
    [ApiController]
    public class ParkedController : ControllerBase
    {
        public readonly ParkingContext db;
        public readonly ParkedService service;

        public ParkedController(ParkingContext db)
        {
            this.db = db;
            this.service = new ParkedService(this.db);
        }

        [Route("GetBusySlots/")]
        [HttpGet]
        public List<ParkedCar> GetBusySlots()
        {
            var p = db.Parked.Include(x => x.Slot)
                                .Include(x => x.Car)
                                .Include(x => x.Car.Model)
                                .Include(x => x.Car.Type)
                                .Include(x => x.Car.Model.Manufacturer)
                                .Where(x => x.Slot.IsBusy)
                                .ToList();
            return p;
        }

        [Route("GetParkedById/")]
        [HttpGet]
        public ParkedCar GetSlotById([FromQuery]string id)
        {
            var p = db.Parked.Include(x => x.Slot)
                                .Include(x => x.Car)
                                .Where(x => x.Id == id)
                                .FirstOrDefault();
            return p;
        }

        [Route("NewParked/")]
        [HttpPost]
        public async Task<ActionResult<ParkedCar>> AsyncNewCarParked([FromBody] Car car)
        {
            var x = await service.AllocateCar(car);
            if (x)
            {
                return CreatedAtAction("Carro alocado com sucesso:", new { id = car.Id }, car);
            }
            else
            {
                return CreatedAtAction("Falha ao alocar carro:", new { id = car.Id }, car);
            }
        }

    }
}
