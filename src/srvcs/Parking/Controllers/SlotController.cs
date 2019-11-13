using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.API.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.API.Controller
{
    [Route("api/[controller]/")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        public readonly ParkingContext db;

        public SlotController(ParkingContext db)
        {
            this.db = db;
        }

        [Route("GetSlots/")]
        [HttpGet]
        public List<Slot> GetAllSlots()
        {
            var p = db.Slots.ToList();
                        
            return p;
        }

        [Route("GetFreeSlots/")]
        [HttpGet]
        public List<Slot> GetFreeSlots()
        {
            var p = db.Slots.Where(x => !x.IsBusy)
                            .ToList();

            return p;
        }

        [Route("GetSlotById/")]
        [HttpGet]
        public Slot GetSlotById([FromQuery]string id)
        {
            var p = db.Slots.Include(x => x.Types)
                            .Where(x => x.Id == id)
                            .FirstOrDefault();
            return p;
        }

        [Route("NewSlot/")]
        [HttpPost]
        public async Task<bool> AsyncAddSlot([FromBody] Slot slot)
        {

            var c = db.Set<Core.Models.Type>().Where(x => slot.Types.Select( y => y.TypeId).Contains(x.Id)).FirstOrDefault();

            if (c == null)
            {
                return false;
            }

            slot.IsBusy = false;

            db.Slots.Add(slot);

            await db.SaveChangesAsync();
            
            return true;
        }

    }
}
