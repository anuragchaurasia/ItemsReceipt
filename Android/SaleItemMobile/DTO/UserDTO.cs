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
    public class UserDTO
    {
        public int StoreUserID { get; set; }
        public string StoreName { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceID { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}