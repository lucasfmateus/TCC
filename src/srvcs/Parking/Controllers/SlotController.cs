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
            var p = db.Slots.Include(x => x.Types)
                            .ToList();

            return p;
        }

        [Route("GetFreeSlots/")]
        [HttpGet]
        public List<Slot> GetFreeSlots()
        {
            var p = db.Slots.Include(x => x.Types)
                            .Where(x => !x.IsBusy)
                            .AsNoTracking()
                            .ToList();

            foreach (var item in p)
            {
                foreach (var x in item.Types)
                {
                    x.Type = db.Types.Where(y => y.Id == x.TypeId).FirstOrDefault();
                }
            }

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

        [Route("NewSlot")]
        [HttpPost]
        public async Task<bool> AddSlotAsync([FromBody] Slot slot)
        { 
            
            try
            {
                var types = db.Set<Core.Models.Type>().Where(x => slot.Types.Select(y => y.TypeId).Contains(x.Id)).AsNoTracking().ToList();
                
                var s = db.Set<Slot>().Where(x => x.Id == slot.Id).FirstOrDefault();
                
                slot.IsBusy = false;

                if (s == null)
                {
                    slot.GenerateId();

                    slot.Types = new List<SlotType>();

                    List<SlotType> aux = new List<SlotType>();

                    foreach (var type in types)
                    {
                        aux.Add(
                            new SlotType()
                            {
                                TypeId = type.Id,
                                SlotId = slot.Id
                            });
                    }

                    slot.Types = aux;

                    db.Slots.Add(slot);


                }
                else
                {
                    s.Name = slot.Name;

                    if(s.Types != null)
                    {
                        s.Types = slot.Types;
                    }
                }

                await db.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [Route("DeleteSlot")]
        [HttpDelete]
        public async Task DeleteSlotAsync([FromQuery] string slotId)
        {
            try
            {
                var p = db.Set<Slot>().Where(x => x.Id == slotId).FirstOrDefault();

                if (p == null)
                {
                    throw new Exception("Vaga não encontrada no Banco.");
                }


                var a = db.Set<ParkedCar>().Where(x => x.Slot.Id == p.Id).AsNoTracking().FirstOrDefault();

                if (a != null)
                {
                    throw new Exception("Não foi possivel deletar esta vaga. Existe um veículo alocado nessa vaga.");
                }

                db.Slots.Remove(p);
                
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
