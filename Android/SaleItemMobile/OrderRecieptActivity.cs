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
using SaleItemMobile.Helper;
using System.Json;
using SaleItemMobile.SaleEntities;
using SaleItemMobile.DTO;
using SaleItemMobile.ListAdapters;
using Newtonsoft.Json;

namespace SaleItemMobile
{
    [Activity(Label = "OrderRecieptActivity")]
    public class OrderRecieptActivity : Activity
    {
        RecieptAdapter recieptAdapter;
        private List<RecieptDTO> mlist;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OrderReciept);
            SessionHelper sessionHelper = new SessionHelper(this);
            string Username = sessionHelper.GetSessionKey(SessionKeys.SESSION_USERNAME_KEY);
            string UserID = sessionHelper.GetSessionKey(SessionKeys.SESSION_USERID_KEY);
            string AuthToken = sessionHelper.GetSessionKey(SessionKeys.SESSION_TOKEN_KEY);

            JsonValue jsonReciept = await HttpRequestHelper<RecieptEntity>.GetRequest(ServiceTypes.GetOrderedRecieptsForStore, "/" + AuthToken + "/" + UserID);
            ParseRecieptJSON(jsonReciept);
        }

        private void ParseRecieptJSON(JsonValue json)
        {
            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue recieptResults = json;
            List<string> itemList = new List<string>();
            // Extract the "stationName" (location string) and write it to the location TextBox:
            bool isSuccess = recieptResults["IsSuccess"];
            if (isSuccess)
            {
                JsonArray recieptArray = (JsonArray)recieptResults["reciepts"];
                List<RecieptDTO> output = new List<RecieptDTO>();
                foreach (JsonValue item in recieptArray)
                {
                    RecieptDTO d = new RecieptDTO();
                    d.Name = item["Name"];
                    d.RecieptID = item["RecieptID"];
                    output.Add(d);
                }
                mlist = output;
                var listView = FindViewById<ListView>(Resource.Id.lstOrderedReciepts);
                listView.ItemClick += ListView_ItemClick;
                recieptAdapter = new RecieptAdapter(this, mlist);
                listView.Adapter = recieptAdapter;
                Toast.MakeText(this, "Ordered reciepts recieved successfully.", ToastLength.Short).Show();
            }
            else
            {
                string errorMessage = recieptResults["Message"];
                Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
            }
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = mlist[e.Position].Name + " " + mlist[e.Position].RecieptID;
            Intent intent = new Intent(this, typeof(OrderedProductActivity));
            RecieptDTO extras = new RecieptDTO();
            extras.RecieptID = mlist[e.Position].RecieptID;
            extras.Name = mlist[e.Position].Name;
            intent.PutExtra("RecieptData", JsonConvert.SerializeObject(extras));
            StartActivity(intent);

            Android.Widget.Toast.MakeText(this, select, Android.Widget.ToastLength.Long).Show();
        }
    }
}