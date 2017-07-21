using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityLayer
{
    public class ReceiptOrderEL
    {
        public int RecieptOrderID { get; set; }
        public Nullable<int> RecieptID { get; set; }
        public Nullable<int> SenderStoreID { get; set; }
        public string OrderTime { get; set; }
        public Nullable<int> ReceiverStoreID { get; set; }
    }
}
