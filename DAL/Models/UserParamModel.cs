using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class UserParamModel
    {
        public int Id { get; set; } = -1;
        public int LanguageId { get; set; } = -1;
        public Boolean IsActive { get; set; } = true;
        public string FullName { get; set; }  ="";
        public string AspNetUsersId { get; set; } = "";
        public string IMe { get; set; } = "";
        public int Age_Id { get; set; } = 1;
        public int Gender_Id { get; set; } = 1;
        public int Location_id { get; set; } = 1;
        public int Nationality_Id { get; set; } = 1;
        public int School_Id { get; set; } = 1;
        public int SchoolType_id { get; set; } = 1;
    }
}
