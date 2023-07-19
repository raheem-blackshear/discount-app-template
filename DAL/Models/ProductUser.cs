using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class ProductUser
    {
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
