using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Questions
{
    public class Question
    {
        public int Id { get; set; }
        public int Age { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionOnlineShopping> QuestionOnlineShoppings { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionPreferredStyle> QuestionPreferredStyles { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionProductType> QuestionProductTypes { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionShoppingTime> QuestionShoppingTimes { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionCategory> QuestionCategories { get; set; }

    }
}
