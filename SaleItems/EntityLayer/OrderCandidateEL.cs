using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityLayer
{
    public class OrderCandidateEL
    {
        public int OrderCandidateID { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<bool> IsAvailable { get; set; }
        public string Price { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
