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

namespace SaleItemMobile.DTO
{
    public class RecieptDTO
    {
        public int RecieptID { get; set; }
        public string Name { get; set; }
        public Nullable<int> StoreID { get; set; }
        public string CreatedOn { get; set; }
        public string Status { get; set; }
    }
}