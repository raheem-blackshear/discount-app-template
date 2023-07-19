using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Image : AuditableEntity
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
    }
}
