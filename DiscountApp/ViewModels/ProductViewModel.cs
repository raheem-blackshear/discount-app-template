using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountApp.ViewModels
{
    public class ProductViewModel
    {
        public List<ProductReturnResult> Sale { get; set; }
        public List<ProductReturnResult> Trending { get; set; }
        public List<ProductReturnResult> New { get; set; }
    }
}
