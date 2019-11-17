using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class SlotDTO
    {
        public List<TypesDTO> Types { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public double DistDoor { get; set; }
        public bool IsBusy { get; set; }
        public SlotDTO(Slot slot)
        {
            this.Id = slot.Id;
            this.Name = slot.Name;
            this.DistDoor = slot.DistDoor;
            this.IsBusy = slot.IsBusy;
            this.Types = ConvertTypes(slot);

        }
        
        public SlotDTO()
        {
        }

        public List<TypesDTO> ConvertTypes(Slot slot )
        {
            List<TypesDTO> listAux = new List<TypesDTO>();
            foreach (var item in slot.Types)
            {
                listAux.Add
                (
                    new TypesDTO(item.Type)
                );
            }

            Types = listAux;
            return Types;
        }
    }

    public class TypesDTO
    {
        public string Name{ get; set; }
        public string Id{ get; set; }
        public TypesDTO(Type type)
        {
            this.Name = type.Name;
            this.Id = type.Id;
        }
        public TypesDTO()
        {
        }
    }
}
