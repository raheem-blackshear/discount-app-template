using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product : AuditableEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        //[NotMapped]
        //public string IconURL
        //{
        //    get
        //    {
        //        return Path.Combine("https://discountapp20190105022247.azurewebsites.net/images/", Icon);
        //        ;
        //    }
        //}

        public decimal Price { get; set; }


        public decimal SellingPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool IsActive { get; set; }
        public virtual int StoreId { get; set; }
        public virtual int? CategoryId { get; set; }

        public bool IsOnSale { get; set; }
        public bool IsTrending { get; set; }

        public DateTime DealDate { get; set; }

     
      // [NotMapped]
     //  public bool IsFollowed { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductUser> ProductUsers { get; set; }


    }
}
