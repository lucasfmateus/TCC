using Core.Models;
using Microsoft.EntityFrameworkCore;
using Parking.API.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.API.Services
{
    public class ParkedService
    {
        public readonly ParkingContext db;

        public ParkedService(ParkingContext db)
        {
            this.db = db;
        }

        public async Task<Slot> AllocateCar(Car car)
        {
            try
            {
                var carDb = db.Set<Car>()
                    .Include(x => x.Model)
                    .Include(x => x.Type)
                    .Include(x => x.Model.Manufacturer)
                    .Where(x => x.Id == car.Id)
                    .FirstOrDefault();

                var slotType = db.Set<SlotType>()
                            .Include(x => x.Slot)
                            .Include(x => x.Type)
                            .OrderBy(x => x.Slot.DistDoor)
                            .Where(t => t.Type.Name == car.Type.Name && t.Slot.IsBusy == false && t.Slot.Types.Count() == 1)
                            .FirstOrDefault();

                var slot = slotType?.Slot;      
                
                if(slot == null)
                {
                    slotType = db.Set<SlotType>()
                        .Include(x => x.Slot)
                        .Include(x => x.Type)
                        .OrderBy(x => x.Slot.DistDoor)
                        .Where(t => t.Type.Name == car.Type.Name && t.Slot.IsBusy == false)
                        .FirstOrDefault();
                }

                slot = slotType?.Slot;

                if (carDb == null || slot == null)
                {
                    return null;
                }

                slot.IsBusy = true;

                db.Parked.Add(
                    new ParkedCar
                    {
                        Car = carDb,
                        Slot = slot,
                        CreateAt = DateTimeOffset.Now
                    });

                await db.SaveChangesAsync();

                return slot;

            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

    }

}
