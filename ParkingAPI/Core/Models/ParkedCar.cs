using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ParkedCar : BaseEntity
    {

        public Car Car { get; set; }

        public Slot Slot { get; set; }

    }
}
