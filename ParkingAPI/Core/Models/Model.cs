using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Model : BaseEntity
    {
        public string Name { get; set; }

        public Manufacturer Manufacturer { get; set; }
    }
}
