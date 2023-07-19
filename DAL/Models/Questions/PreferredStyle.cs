using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Questions
{
    public class PreferredStyle
    {

        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionPreferredStyle> QuestionPreferredStyles { get; set; }
    }
}
