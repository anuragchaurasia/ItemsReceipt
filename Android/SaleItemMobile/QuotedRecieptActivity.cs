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
using SaleItemMobile.ListAdapters;
using SaleItemMobile.DTO;
using SaleItemMobile.Helper;
using System.Json;
using Newtonsoft.Json;
using SaleItemMobile.SaleEntities;

namespace SaleItemMobile
{
    [Activity(Label = "QuotedRecieptActivity")]
    public class QuotedRecieptActivity : Activity
    {
        RecieptAdapter recieptAdapter;
        private List<RecieptDTO> mlist;
        private List<UserDTO> ulist;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.QuotedReciept);
            TextView storeUsernameText = FindViewById<TextView>(Resource.Id.txtUsername);
            SessionHelper sessionHelper = new SessionHelper(this);
            string Username = sessionHelper.GetSessionKey(SessionKeys.SESSION_USERNAME_KEY);
            string UserID = sessionHelper.GetSessionKey(SessionKeys.SESSION_USERID_KEY);
            string AuthToken = sessionHelper.GetSessionKey(SessionKeys.SESSION_TOKEN_KEY);
            storeUsernameText.Text = "Hello " + Username;
            JsonValue jsonReciept = await HttpRequestHelper<QuotedEntity>.GetRequest(ServiceTypes.GetQuotedRecieptsForCustomer, "/" + AuthToken + "/" + UserID);
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
                JsonArray userArray = (JsonArray)recieptResults["users"];
                List<RecieptDTO> output = new List<RecieptDTO>();
                foreach (JsonValue item in recieptArray)
                {
                    RecieptDTO d = new RecieptDTO();
                    d.Name = item["Name"];
                    d.RecieptID = item["RecieptID"];
                    output.Add(d);
                }
                mlist = output;

                List<UserDTO> userOutput = new List<UserDTO>();
                foreach (JsonValue item in userArray)
                {
                    UserDTO d = new UserDTO();
                    d.StoreName = item["StoreName"];
                    d.StoreUserID = item["StoreUserID"];
                    userOutput.Add(d);
                }
                ulist = userOutput;


                var listView = FindViewById<ListView>(Resource.Id.lstQuotedReciept);
                listView.ItemClick += ListView_ItemClick;
                recieptAdapter = new RecieptAdapter(this, mlist);
                listView.Adapter = recieptAdapter;
                Toast.MakeText(this, "Quoted reciepts recieved successfully.", ToastLength.Short).Show();
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
            Intent intent = new Intent(this, typeof(QuotedStoreActivity));
            intent.PutExtra("UserData", JsonConvert.SerializeObject(ulist));
            StartActivity(intent);

            Android.Widget.Toast.MakeText(this, select, Android.Widget.ToastLength.Long).Show();
        }
    }
}