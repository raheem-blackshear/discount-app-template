using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class UserModel : AuditableEntity
    {

        
        [Key]
        public int User_Id { get; set; }
        public string FullName { get; set; }
        public string AspNetUsersId { get; set; }
      
        public string FaceBook_Id { get; set; }
  
        public Boolean IsActive { get; set; }

        public string ProfilePicture { get; set; }

        public int? Age { get; set; }

        public int? GenderId { get; set; }

        public string Email { get; set; }
        public Location Location { get; set; }

        public int? QuestionId { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductUser> ProductUsers { get; set; }

        [JsonIgnore]
        public virtual ICollection<StoreUser> StoreUsers { get; set; }
    }
}
