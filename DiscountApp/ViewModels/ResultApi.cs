using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountApp.ViewModels
{
    public class ResultApi
    {
        #region Properties
        public string Result { get; set; }

        public string Data { get; set; }

        public UserModel LstData { get; set; }

        public string Message { get; set; }
        #endregion
    }

    public class ProductResultApi
    {
        #region Properties
        public string Result { get; set; }

        public List<ProductReturnResult> DataList { get; set; }

        
        #endregion
    }
}