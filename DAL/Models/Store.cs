using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Spatial;
using System.Text;

namespace DAL.Models
{
   public class Store:AuditableEntity

    {
        public int Id { get; set; }

        [JsonIgnore]
        public virtual int? CategoryId { get; set; }

        public string Name { get; set; }
        
        public string Icon { get; set; }
        public string Banner { get; set; }

        public string Slogan { get; set; }

        public string SaleDetails { get; set; }

        public bool Recommended { get; set; }
        public bool Featured { get; set; }

        

        [NotMapped]
        public virtual bool isFollowed { get; set; }
        

        public Category Category { get; set; }

        [JsonIgnore]
        public virtual List<Product> Products { get; set; }
        public List<Location> Location { get; set; }
        public List<Lookbook> Lookbook { get; set; }

        [NotMapped]
        public virtual List<Style> Styles { get; set; }

        [JsonIgnore]
        public virtual ICollection<StoreStyle> StoreStyles { get; set; }

        [JsonIgnore]
        public virtual ICollection<StoreUser> StoreUsers { get; set; }
    }
}
