using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Questions
{
    public class ShoppingTime
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionShoppingTime> QuestionShoppingTimes { get; set; }
    }
}
