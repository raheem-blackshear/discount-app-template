﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class Location : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }

        public virtual int? StoreId { get; set; }

        [JsonIgnore]
        public virtual int? UserId { get; set; }

  
    }
}