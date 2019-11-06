using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Car : BaseEntity
    {
        public Model Model { get; set; }

        public Type Type { get; set; }
    }

}
