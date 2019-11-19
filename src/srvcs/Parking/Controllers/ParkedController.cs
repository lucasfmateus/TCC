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

        [Route("GetBusySlots")]
        [HttpGet]
        public List<ParkedCar> GetBusySlots()
        {
            var busy = db.Parked.Include(x => x.Slot)
                                .Include(x => x.Car)
                                .Include(x => x.Car.Model)
                                .Include(x => x.Car.Type)
                                .Include(x => x.Car.Model.Manufacturer)
                                .Where(x => x.Slot.IsBusy)
                                .ToList();
            return busy;
        }

        [Route("GetParkedById/")]
        [HttpGet]
        public ParkedCar GetSlotById([FromQuery]string id)
        {
            var parked = db.Parked.Include(x => x.Slot)
                                .Include(x => x.Car)
                                .Where(x => x.Id == id)
                                .FirstOrDefault();
            return parked;
        }

        [Route("NewParked")]
        [HttpPost]
        public async Task<SlotDTO> AsyncNewCarParked([FromBody] Car car)
        {
            return await service.AllocateCar(car);
        }

        [Route("DeleteParked")]
        [HttpDelete]
        public async Task DeleteParkedAsync([FromQuery] string id)
        {
            try
            {
                var parked = db.Parked.Include(x => x.Slot)
                                    .Include(x => x.Car)
                                    .Where(x => x.Id == id)
                                    .FirstOrDefault();

                if (parked == null)
                {
                    throw new Exception("Carro não encontrado no Banco.");
                }

                var parkedCar = db.Set<ParkedCar>().Where(x => x.Car.Id == parked.Car.Id).AsNoTracking().ToList();

                if (parkedCar != null)
                {
                    throw new Exception("Já possuiu um registro desse carro estacionado.");
                }

                db.Parked.Remove(parked);

                var slot = db.Slots.Where(x => x.Id == parked.Slot.Id).FirstOrDefault();

                slot.IsBusy = false;

                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }

    }
}
