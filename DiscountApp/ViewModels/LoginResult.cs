using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountApp.ViewModels
{
    public class LoginResult
    {
        public string USER_ID { get; set; } = "";
        public string FULLNAME { get; set; } = "";

        public int? Age { get; set; }
        public int? GenderId { get; set; }
        public string ProfilePicture { get; set; }
        public string LocationId { get; set; }
        public string Email { get; set; }

        public string AspNetUsersId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
