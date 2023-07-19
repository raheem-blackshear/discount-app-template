using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Questions
{
    public class QuestionOnlineShopping
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int OnlineId { get; set; }
        public OnlineShopping OnlineShopping { get; set; }

    }


    public class QuestionPreferredStyle
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int PreferredId { get; set; }
        public PreferredStyle PreferredStyle { get; set; }

    }

    public class QuestionProductType
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

    }

    public class QuestionShoppingTime
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int ShoppingTimeId { get; set; }
        public ShoppingTime ShoppingTime { get; set; }

    }

    public class QuestionCategory
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
