using System.Collections.Generic;

namespace DAL.Models
{
    public class ProductImage : AuditableEntity
    {
        public int Id { get; set; }
        public string DownloadUrl { get; set; }
        
        public int ProductId { get; set; }

}
}