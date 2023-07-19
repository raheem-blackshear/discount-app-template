using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountApp.ViewModels
{
    public class ProductStoreViewModel
    {

        int Id { get; set; }
        double price { get; set; }
        double discountedPrice { get; set; }
        string name { get; set; }

        string storeName { get; set; }

        int storeId { get; set; }


    }
}
