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
using SaleItemMobile.SaleEntities;
using System.Json;
using SaleItemMobile.Helper;
using SaleItemMobile.ListAdapters;
using Newtonsoft.Json;
using Android.Support.V4.Widget;

namespace SaleItemMobile
{
    [Activity(Label = "ProductActivity", Theme = "@style/Theme.DesignDemo")]
    public class ProductActivity : Activity
    {
        Android.App.ProgressDialog progress;
        ProductAdapter productAdapter;
        private List<ProductDTO> mlist;
        string textRecieptID = "";
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            //View contentView = inflater.Inflate(Resource.Layout.Product, null, false);
            //drawerLayout.AddView(contentView);
            SetContentView(Resource.Layout.Product);
            RecieptDTO user = JsonConvert.DeserializeObject<RecieptDTO>(Intent.GetStringExtra("RecieptData"));
            //var extras = Intent.GetParcelableExtra("RecieptData") ?? String.Empty;
            string textRecieptName = user.Name;
            textRecieptID = user.RecieptID.ToString();
            TextView txtRecieptName = FindViewById<TextView>(Resource.Id.recieptTxt);
            TextView txtProductName = FindViewById<TextView>(Resource.Id.editProductName);
            TextView txtProductQuantity = FindViewById<TextView>(Resource.Id.editProductQuantity);
            txtRecieptName.Text = textRecieptName;
            SessionHelper sessionHelper = new SessionHelper(this);
            string Username = sessionHelper.GetSessionKey(SessionKeys.SESSION_USERNAME_KEY);
            string UserID = sessionHelper.GetSessionKey(SessionKeys.SESSION_USERID_KEY);
            string AuthToken = sessionHelper.GetSessionKey(SessionKeys.SESSION_TOKEN_KEY);

            ImageButton btnAddProduct = FindViewById<ImageButton>(Resource.Id.btnAddProduct);
            JsonValue jsonProduct = await HttpRequestHelper<ProductEntity>.GetRequest(ServiceTypes.GetProducts, "/" + AuthToken + "/" + textRecieptID);
            ParseRecieptJSON(jsonProduct);

            Button btnSendReceipt = FindViewById<Button>(Resource.Id.btnSendReceipt);

            btnSendReceipt.Click += BtnSendReceipt_Click;

            btnAddProduct.Click += async (sender, e) =>
        {
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Adding product... Please wait...");
            progress.SetCancelable(false);
            progress.Show();
            try
            {
                ProductDTO productDTO = new ProductDTO();
                productDTO.AddedOn = DateTime.Now.ToString();
                productDTO.Name = txtProductName.Text;
                productDTO.Quantity = txtProductQuantity.Text;
                productDTO.RecieptID = Convert.ToInt32(textRecieptID);
                ProductEntity productEntity = new ProductEntity { AuthToken = AuthToken, productInfo = productDTO };
                JsonValue json = await HttpRequestHelper<ProductEntity>.POSTreq(ServiceTypes.AddProduct, productEntity);
                ParseJSON(json);

                JsonValue jsonProduct1 = await HttpRequestHelper<ProductEntity>.GetRequest(ServiceTypes.GetProducts, "/" + AuthToken + "/" + textRecieptID);
                ParseRecieptJSON(jsonProduct1);
                progress.Hide();
            }
            catch (Exception ex)
            {

            }
        };
        }

        private void BtnSendReceipt_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(StoreActivity));
            intent.PutExtra("RecieptID", textRecieptID);
            StartActivity(intent);

        }

        private void ParseJSON(JsonValue json)
        {
            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue loginResults = json;

            // Extract the "stationName" (location string) and write it to the location TextBox:
            bool isSuccess = loginResults["IsSuccess"];
            if (isSuccess)
            {

                Toast.MakeText(this, "Product added successfully.", ToastLength.Short).Show();
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
            JsonValue productsResults = json;
            List<string> itemList = new List<string>();
            // Extract the "stationName" (location string) and write it to the location TextBox:
            bool isSuccess = productsResults["IsSuccess"];
            if (isSuccess)
            {
                JsonArray recieptArray = (JsonArray)productsResults["products"];
                List<ProductDTO> output = new List<ProductDTO>();
                foreach (JsonValue item in recieptArray)
                {
                    ProductDTO d = new ProductDTO();
                    d.Name = item["Name"];
                    d.Quantity = item["Quantity"];
                    output.Add(d);
                }
                mlist = output;
                var listView = FindViewById<ListView>(Resource.Id.listProduct);
                productAdapter = new ProductAdapter(this, mlist);
                listView.Adapter = productAdapter;
                Toast.MakeText(this, "Product recieved successfully.", ToastLength.Short).Show();
            }
            else
            {
                string errorMessage = productsResults["Message"];
                Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
            }
        }

    }
}