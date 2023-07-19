using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class ProductReturnResult
    {

        public Product product;

        public string storeName { get; set; }

        public int storeId { get; set; }

        public List<Location> location { get; set; }

        public int categoryId { get; set; }
        public string categoryName { get; set; }

        public bool? isFollowed { get; set; }

    }
}
