using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Questions
{
    public class ProductType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionProductType> QuestionProductTypes { get; set; }
    }
}
