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
    public class ProductDTO
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