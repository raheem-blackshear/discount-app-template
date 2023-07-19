using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountApp.ViewModels
{
    public class HomeViewModel
    {

        public List<ProductReturnResult> Sale { get; set; }
        public List<ProductReturnResult> Trending { get; set; }
        public ProductReturnResult Deal { get; set; }
    }

    public class StoreHomeViewModel
    {

        public List<Store> Featured { get; set; }
        public List<Store> Recommended { get; set; }
        public List<Store> Deals { get; set; }
    }
}
