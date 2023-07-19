using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    
        public class StoreUser
        {
            public int UserId { get; set; }
            public UserModel User { get; set; }
            public int StoreId { get; set; }
            public Store Store { get; set; }
        
    }
}
