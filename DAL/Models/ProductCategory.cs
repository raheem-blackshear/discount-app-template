using DAL.Models.Questions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Models
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
      
       // [JsonIgnore]
       // public ICollection<Product> Products { get; set; }

        [JsonIgnore]
        public ICollection<Store> Stores { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionCategory> QuestionCategories { get; set; }
    }
}
