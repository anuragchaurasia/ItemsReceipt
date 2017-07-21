using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SaleItemMobile.SaleEntities
{
    public class RecieptOrderEntity : BaseEntity
    {
        public int RecieptOrderID { get; set; }
        public int RecieptID { get; set; }
        public int SenderStoreID { get; set; }
        public string OrderTime { get; set; }
        public int ReceiverStoreID { get; set; }
    }
}