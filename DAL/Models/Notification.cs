using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string User_Id { get; set; }
       
        public string NotifiedUser_Id { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public Boolean Read { get; set; }
        public Boolean isBroadcast { get; set; }

        //  [JsonIgnore]
        public virtual int StoreId { get; set; }

     
    }
}
