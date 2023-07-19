namespace DAL.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class Lookbook : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string Icon { get; set; }

       // [JsonIgnore]
        public virtual int StoreId { get; set; }

       
    }
}