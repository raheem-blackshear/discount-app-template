using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountApp.ViewModels
{
    public class UpdateUsers
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string AspNetUsersId { get; set; }

        public string Email { get; set; }
        public string FaceBook_Id { get; set; }

        public Nullable<int> Age { get; set; }
        public Nullable<int> GenderId { get; set; }
        public string ProfileImage { get; set; }


        public Boolean IsActive { get; set; }
    }
}
