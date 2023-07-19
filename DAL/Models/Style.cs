using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class Style : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

     

      //  public virtual int StoreId { get; set; }

        [JsonIgnore]
        public virtual ICollection<StoreStyle> StoreStyles { get; set; }
    }
}