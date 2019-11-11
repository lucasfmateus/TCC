using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Manufacturer : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
