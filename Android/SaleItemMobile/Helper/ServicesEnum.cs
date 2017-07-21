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

namespace SaleItemMobile.Helper
{
    public enum ServiceTypes
    {
        Login,
        RegisterStore,
        AddReciept,
        AddProduct,
        GetProducts,
        GetReciepts,
        GetAllUsers,
        UpdatePushToken,
        UpdateProductAvailability,
        GetOrderedRecieptsForStore,
        UpdateRecieptStatus,
        AddOrderReciept,
        GetQuotedRecieptsForCustomer
    }
}