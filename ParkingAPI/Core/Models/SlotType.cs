using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class SlotType
    {
        public string SlotId { get; set; }

        public Slot Slot { get; set; }

        public string TypeId { get; set; }

        public Type Type { get; set; }
    }
}
