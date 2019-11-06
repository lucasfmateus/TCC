using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Slot : BaseEntity
    {
        public string Name { get; set; }

        public double DistDoor { get; set; }

        public bool IsBusy { get; set; }

        public List<SlotType> Types { get; set; }
    }
}
