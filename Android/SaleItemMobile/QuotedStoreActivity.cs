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
using Newtonsoft.Json;
using System.Json;
using SaleItemMobile.ListAdapters;

namespace SaleItemMobile
{
    [Activity(Label = "QuotedStoreActivity")]
    public class QuotedStoreActivity : Activity
    {
        StoreAdapter storeAdapter;
        private List<UserDTO> ulist;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.QuotedStore);
            string json = Intent.GetStringExtra("UserData");
            ulist = JsonConvert.DeserializeObject<List<UserDTO>>(json);
            var listView = FindViewById<ListView>(Resource.Id.listQuotedStores);
            storeAdapter = new StoreAdapter(this, ulist);
            listView.Adapter = storeAdapter;
            Toast.MakeText(this, "Store recieved successfully.", ToastLength.Short).Show();
        }
    }
}