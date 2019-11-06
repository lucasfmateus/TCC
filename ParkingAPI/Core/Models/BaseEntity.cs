using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public DateTimeOffset CreateAt { get; set; }
    }
}
