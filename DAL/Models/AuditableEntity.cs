﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Interfaces;
using Newtonsoft.Json;

namespace DAL.Models
{
   
    public class AuditableEntity : IAuditableEntity
    {
        [MaxLength(256)]
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [MaxLength(256)]
        [JsonIgnore]
        public string UpdatedBy { get; set; }

        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
    }
}
