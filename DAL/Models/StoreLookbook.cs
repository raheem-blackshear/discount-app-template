


namespace DAL.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Text;
    public class StoreStyle
    {
        public int StyleId { get; set; }
        public Style Style { get; set; }

        
        public int StoreId { get; set; }
        public Store Store { get; set; }

    }
}
