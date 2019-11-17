using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class Slot : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Required]
        public string Name { get; set; }
        [Required]
        public double DistDoor { get; set; }
        [Required]
        public bool IsBusy { get; set; }
        
        public List<SlotType> Types { get; set; }
    }
}
