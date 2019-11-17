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
    public class ParkedController : ControllerBase
    {
        public readonly ParkingContext db;
        public readonly ParkedService service;

        public ParkedController(ParkingContext db)
        {
            this.db = db;
            this.service = new ParkedService(this.db);
            db.InitializeDatabase();
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
        public async Task<Slot> AsyncNewCarParked([FromBody] Car car)
        {
            return await service.AllocateCar(car);
        }

    }
}
