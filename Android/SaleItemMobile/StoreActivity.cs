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
using System.Json;
using SaleItemMobile.Helper;
using SaleItemMobile.SaleEntities;
using Android.Support.V4.Widget;
using System.Threading.Tasks;

namespace SaleItemMobile
{
    [Activity(Label = "Stores", Theme = "@style/Theme.DesignDemo")]
    public class StoreActivity : Activity
    {
        Android.App.ProgressDialog progress;
        StoreAdapter storeAdapter;
        private List<UserDTO> ulist;
        string RecieptID, AuthToken;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            //View contentView = inflater.Inflate(Resource.Layout.Stores, null, false);
            //drawerLayout.AddView(contentView);
            SetContentView(Resource.Layout.Stores);
            RecieptID = Intent.GetStringExtra("RecieptID");
            SessionHelper sessionHelper = new SessionHelper(this);
            AuthToken = sessionHelper.GetSessionKey(SessionKeys.SESSION_TOKEN_KEY);

            JsonValue jsonProduct = await HttpRequestHelper<StoreEntity>.GetRequest(ServiceTypes.GetAllUsers, "/" + AuthToken);
            ParseStoreJSON(jsonProduct);
        }

        private void ParseStoreJSON(JsonValue json)
        {
            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue storeResults = json;
            List<string> itemList = new List<string>();
            // Extract the "stationName" (location string) and write it to the location TextBox:
            bool isSuccess = storeResults["IsSuccess"];
            if (isSuccess)
            {
                JsonArray recieptArray = (JsonArray)storeResults["userDetails"];
                List<UserDTO> output = new List<UserDTO>();
                foreach (JsonValue item in recieptArray)
                {
                    UserDTO d = new UserDTO();
                    d.StoreName = item["StoreName"];
                    d.Username = item["Username"];
                    d.StoreUserID = item["StoreUserID"];
                    output.Add(d);
                }
                ulist = output;
                var listView = FindViewById<ListView>(Resource.Id.listStores);
                storeAdapter = new StoreAdapter(this, ulist);
                listView.ItemClick += ListView_ItemClick;
                listView.Adapter = storeAdapter;
                Toast.MakeText(this, "Store recieved successfully.", ToastLength.Short).Show();
            }
            else
            {
                string errorMessage = storeResults["Message"];
                Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
            }
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            RecieptOrderEntity recieptOrder = new RecieptOrderEntity();
            recieptOrder.AuthToken = AuthToken;
            recieptOrder.OrderTime = DateTime.Now.ToString();
            recieptOrder.ReceiverStoreID = ulist[e.Position].StoreUserID;
            recieptOrder.RecieptID = Convert.ToInt32(RecieptID);
            recieptOrder.SenderStoreID = 0;
            Task<JsonValue> jsonRecieptSent = HttpRequestHelper<RecieptOrderEntity>.POSTreq(ServiceTypes.AddOrderReciept, recieptOrder);
            ParseOrderRecieptJSON(jsonRecieptSent.Result);
        }

        private void ParseOrderRecieptJSON(JsonValue json)
        {
            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue storeResults = json;
            List<string> itemList = new List<string>();
            // Extract the "stationName" (location string) and write it to the location TextBox:
            bool isSuccess = storeResults["IsSuccess"];
            if (isSuccess)
            {
                Toast.MakeText(this, "Reciept sent to store successfully.", ToastLength.Short).Show();
            }
            else
            {
                string errorMessage = storeResults["Message"];
                Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
            }
        }
    }
}