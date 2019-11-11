using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Model : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Manufacturer Manufacturer { get; set; }
    }
}
