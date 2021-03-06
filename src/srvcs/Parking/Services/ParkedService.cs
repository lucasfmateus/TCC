﻿using Core.Models;
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

        public async Task<bool> AllocateCar(Car car)
        {
            var c = db.Set<Car>()
                    .Include(x => x.Model)
                    .Include(x => x.Type)
                    .Include(x => x.Model.Manufacturer)
                    .Where(x => x.Id == car.Id)
                    .FirstOrDefault();

            var p = db.Set<SlotType>()
                        .Include(x => x.Slot)
                        .Include(x => x.Type)
                        .OrderBy(x => x.Slot.DistDoor)
                        .Where(t => t.Type.Name == car.Type.Name && t.Slot.IsBusy == false)
                        .FirstOrDefault();

            var v = p?.Slot;

            if (c == null || v == null)
            {
                return false;
            }

            v.IsBusy = true;

            db.Parked.Add(
                new ParkedCar
                {
                    Car = c,
                    Slot = v,
                    CreateAt = DateTimeOffset.Now
                });

            await db.SaveChangesAsync();

            return true;

        }

    }

}
