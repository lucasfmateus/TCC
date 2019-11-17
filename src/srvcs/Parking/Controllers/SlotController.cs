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
        public List<SlotDTO> GetAllSlots()
        {
            var slots = db.Slots.Include(x => x.Types)
                            .ToList();

            List<SlotDTO> slotsDto = new List<SlotDTO>();

            foreach (var item in slots)
            {
                foreach (var x in item.Types)
                {
                    x.Type = db.Types.Where(y => y.Id == x.TypeId).FirstOrDefault();
                }
            }
            foreach (var item in slots)
            {
                slotsDto.Add(
                        new SlotDTO(item)
                    );
            }

            return slotsDto;
        }

        [Route("GetFreeSlots/")]
        [HttpGet]
        public List<SlotDTO> GetFreeSlots()
        {
            var freeSlots = db.Slots.Include(x => x.Types)
                            .Where(x => !x.IsBusy)
                            .ToList();

            List<SlotDTO> freeSlotsDto = new List<SlotDTO>();

            foreach (var item in freeSlots)
            {
                foreach (var x in item.Types)
                {
                    x.Type = db.Types.Where(y => y.Id == x.TypeId).FirstOrDefault();
                }
            }

            foreach (var item in freeSlots)
            {
                freeSlotsDto.Add(
                        new SlotDTO(item)
                    ); 
            }

            return freeSlotsDto;
        }

        [Route("GetSlotById/")]
        [HttpGet]
        public Slot GetSlotById([FromQuery]string id)
        {
            var slot = db.Slots.Include(x => x.Types)
                            .Where(x => x.Id == id)
                            .FirstOrDefault();
            return slot;
        }

        [Route("NewSlot")]
        [HttpPost]
        //Adiciona uma nova vaga ao banco, que contém uma lista de N tipos de carro compatíveis.
        //O mapeamento é feito através de uma tabela que possui como chave primária uma relação do Id do tipo com o Id do Slot
        public async Task<bool> AddSlotAsync([FromBody] Slot slot)
        { 
            
            try
            {
                var types = db.Set<Core.Models.Type>().Where(x => slot.Types.Select(y => y.TypeId).Contains(x.Id)).AsNoTracking().ToList();
                
                var slots = db.Set<Slot>().Where(x => x.Id == slot.Id).FirstOrDefault();
                
                slot.IsBusy = false;

                if (slots == null)
                {
                    slot.GenerateId();

                    slot.Types = new List<SlotType>();

                    List<SlotType> auxList = new List<SlotType>();

                    foreach (var type in types)
                    {
                        auxList.Add(
                            new SlotType()
                            {
                                TypeId = type.Id,
                                SlotId = slot.Id
                            });
                    }

                    slot.Types = auxList;

                    db.Slots.Add(slot);


                }
                else
                {
                    slots.Name = slot.Name;

                    if(slots.Types != null)
                    {
                        slots.Types = slot.Types;
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
                var slot = db.Set<Slot>().Where(x => x.Id == slotId).FirstOrDefault();

                if (slot == null)
                {
                    throw new Exception("Vaga não encontrada no Banco.");
                }


                var alocated = db.Set<ParkedCar>().Where(x => x.Slot.Id == slot.Id).AsNoTracking().FirstOrDefault();

                if (alocated != null)
                {
                    throw new Exception("Não foi possivel deletar esta vaga. Existe um veículo alocado nessa vaga.");
                }

                db.Slots.Remove(slot);
                
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
