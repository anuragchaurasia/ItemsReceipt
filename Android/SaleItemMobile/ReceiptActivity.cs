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
using SaleItemMobile.SaleEntities;
using System.Json;
using SaleItemMobile.DTO;
using SaleItemMobile.ListAdapters;
using Newtonsoft.Json;
using Android.Support.V4.Widget;

namespace SaleItemMobile
{
    [Activity(Label = "Receipt", Theme = "@style/Theme.DesignDemo")]
    public class ReceiptActivity : Activity
    {
        RecieptAdapter recieptAdapter;
        private List<RecieptDTO> mlist;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            //View contentView = inflater.Inflate(Resource.Layout.Reciept, null, false);
            //drawerLayout.AddView(contentView);
            SetContentView(Resource.Layout.Reciept);
            TextView storeUsernameText = FindViewById<TextView>(Resource.Id.txtUsername);
            SessionHelper sessionHelper = new SessionHelper(this);
            string Username = sessionHelper.GetSessionKey(SessionKeys.SESSION_USERNAME_KEY);
            string UserID = sessionHelper.GetSessionKey(SessionKeys.SESSION_USERID_KEY);
            string AuthToken = sessionHelper.GetSessionKey(SessionKeys.SESSION_TOKEN_KEY);
            storeUsernameText.Text = "Hello " + Username;

            EditText recieptName = FindViewById<EditText>(Resource.Id.editrecieptTxt);
            Button btnAddReciept = FindViewById<Button>(Resource.Id.addreciept);

            JsonValue jsonReciept = await HttpRequestHelper<RecieptEntity>.GetRequest(ServiceTypes.GetReciepts, "/" + AuthToken + "/" + UserID);
            ParseRecieptJSON(jsonReciept);

            btnAddReciept.Click += async (sender, e) =>
            {
                try
                {
                    RecieptDTO recieptDTO = new RecieptDTO();
                    recieptDTO.CreatedOn = DateTime.Now.ToString();
                    recieptDTO.Name = recieptName.Text;
                    recieptDTO.StoreID = Convert.ToInt32(UserID);
                    recieptDTO.Status = ((int)ReceiptStatusEnum.New).ToString();
                    RecieptEntity recieptEntity = new RecieptEntity { AuthToken = AuthToken, recieptDTO = recieptDTO };
                    JsonValue json = await HttpRequestHelper<RecieptEntity>.POSTreq(ServiceTypes.AddReciept, recieptEntity);
                    ParseJSON(json);

                    JsonValue jsonReciept1 = await HttpRequestHelper<RecieptEntity>.GetRequest(ServiceTypes.GetReciepts, "/" + AuthToken + "/" + UserID);
                    ParseRecieptJSON(jsonReciept1);

                }
                catch (Exception ex)
                {

                }
            };
        }

        private void ParseJSON(JsonValue json)
        {
            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue loginResults = json;

            // Extract the "stationName" (location string) and write it to the location TextBox:
            bool isSuccess = loginResults["IsSuccess"];
            if (isSuccess)
            {

                Toast.MakeText(this, "Reciept added successfully.", ToastLength.Short).Show();
            }
            else
            {
                string errorMessage = loginResults["Message"];
                Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
            }
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
                var listView = FindViewById<ListView>(Resource.Id.listView1);
                listView.ItemClick += ListView_ItemClick;
                recieptAdapter = new RecieptAdapter(this, mlist);
                listView.Adapter = recieptAdapter;
                Toast.MakeText(this, "Reciept recieved successfully.", ToastLength.Short).Show();
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
            Intent intent = new Intent(this, typeof(ProductActivity));
            RecieptDTO extras = new RecieptDTO();
            extras.RecieptID = mlist[e.Position].RecieptID;
            extras.Name = mlist[e.Position].Name;
            intent.PutExtra("RecieptData", JsonConvert.SerializeObject(extras));
            StartActivity(intent);

            Android.Widget.Toast.MakeText(this, select, Android.Widget.ToastLength.Long).Show();
        }
    }



}