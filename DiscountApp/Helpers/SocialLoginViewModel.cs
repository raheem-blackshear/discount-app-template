using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountApp.Helpers
{
   public class SocialLoginViewModel
    {

        [EmailAddress]
        public string EMAIL { get; set; }

        public string FACEBOOK_TOKEN { get; set; }

        public string GOOGLE_TOKEN { get; set; }
        
        public int ROLE_ID { get; set; }
        
        public UserModel ItUserModel { get; set; }
    }
}
