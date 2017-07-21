using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityLayer.DTO
{
    public class RecieptDTO
    {
        public int RecieptID { get; set; }
        public string Name { get; set; }
        public Nullable<int> StoreID { get; set; }
        public string CreatedOn { get; set; }
        public string Status { get; set; }
        public string Price { get; set; }
    }
}
