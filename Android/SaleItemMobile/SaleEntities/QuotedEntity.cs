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
using SaleItemMobile.DTO;

namespace SaleItemMobile.SaleEntities
{
    public class QuotedEntity : BaseEntity
    {
        public List<RecieptDTO> reciepts { get; set; }
        public List<UserDTO> users { get; set; }
        public string noReceiptMessage { get; set; }
        public string noUserMessage { get; set; }
    }
}