using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        
        public DateTimeOffset CreateAt { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
}
