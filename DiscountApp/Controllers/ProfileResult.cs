using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiscountApp.ViewModels;

namespace DiscountApp.Controllers
{
    public class ProfileResult 
    {
        public string Result { get; set; }
        public LoginResult Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}