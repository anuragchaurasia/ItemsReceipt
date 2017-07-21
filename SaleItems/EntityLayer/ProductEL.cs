using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityLayer
{
    public class ProductEL
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public Nullable<int> RecieptID { get; set; }
        public Nullable<bool> IsAvailable { get; set; }
        public string Price { get; set; }
    }
}
