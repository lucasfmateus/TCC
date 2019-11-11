using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class Car : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Required]
        public Model Model { get; set; }

        [Required]
        public Type Type { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Folder { get; set; }
    }

}
