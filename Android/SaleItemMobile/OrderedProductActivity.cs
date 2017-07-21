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
using SaleItemMobile.ListAdapters;
using SaleItemMobile.Helper;
using System.Json;
using SaleItemMobile.SaleEntities;

namespace SaleItemMobile
{
    [Activity(Label = "OrderedProductActivity")]
    public class OrderedProductActivity : Activity
    {
        Android.App.ProgressDialog progress;
        OrderedProductAdapter productAdapter;
        private List<ProductDTO> mlist;
        string textRecieptID = "";
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OrderedProduct);
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

            JsonValue jsonProduct = await HttpRequestHelper<ProductEntity>.GetRequest(ServiceTypes.GetProducts, "/" + AuthToken + "/" + textRecieptID);
            ParseRecieptJSON(jsonProduct);

            Button SendReceiptToCustomer = FindViewById<Button>(Resource.Id.btnSendReceiptToCustomer);
            Button UpdateSubTotal = FindViewById<Button>(Resource.Id.btnUpdateOrderAmount);
            UpdateSubTotal.Click += UpdateSubTotal_Click;
            SendReceiptToCustomer.Click += async (sender, e) => {
                
                UpdateProductEntity updateProductEntity = new UpdateProductEntity { AuthToken = AuthToken, productInfo = GetProductsData() };
                JsonValue json = await HttpRequestHelper<UpdateProductEntity>.POSTreq(ServiceTypes.UpdateProductAvailability, updateProductEntity);
                ParseJSON(json);
            };
        }

        private void UpdateSubTotal_Click(object sender, EventArgs e)
        {
            double subtotal = GetSubTotal();
            TextView txtSubTotal = FindViewById<TextView>(Resource.Id.txtSubtotal);
            txtSubTotal.Text = subtotal.ToString() + " INR";
        }

        private void ParseJSON(JsonValue json)
        {
            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue registrationResults = json;

            // Extract the "stationName" (location string) and write it to the location TextBox:
            bool isSuccess = registrationResults["IsSuccess"];

            if (isSuccess)
            {
                Toast.MakeText(this, "Products prices updated and bill sent to customer for confirmation.", ToastLength.Short).Show();

            }
            else
            {
                string errorMessage = registrationResults["Message"];
                Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
            }
        }

        private List<ProductDTO> GetProductsData()
        {
            List<ProductDTO> products = new List<ProductDTO>();

            EditText et;
            TextView tv;

            ListView listview = FindViewById<ListView>(Resource.Id.listOrderedProduct);

            for (int i = 0; i < listview.ChildCount; i++)
            {
                ProductDTO product = new ProductDTO();
                et = (EditText)listview.GetChildAt(i).FindViewById(Resource.Id.editProductPrice);
                tv = (TextView)listview.GetChildAt(i).FindViewById(Resource.Id.productID);
                if (et != null)
                {
                    if (String.IsNullOrEmpty(et.Text))
                    {
                        product.IsAvailable = false;
                        product.Price = "0";
                    }
                    else
                    {
                        product.IsAvailable = true;
                        product.Price = et.Text;
                    }
                }
                if (tv != null)
                {
                    product.ProductID = Convert.ToInt32(tv.Text);
                }
                products.Add(product);
            }


            return products;
        }

        private double GetSubTotal()
        {
            EditText et;
            double subtotal = 0.00;

            ListView listview = FindViewById<ListView>(Resource.Id.listOrderedProduct);

            for (int i = 0; i < listview.ChildCount; i++)
            {
                et = (EditText)listview.GetChildAt(i).FindViewById(Resource.Id.editProductPrice);
                if (et != null)
                {
                    if (String.IsNullOrEmpty(et.Text))
                    {
                        subtotal = subtotal + 0.00;
                    }
                    else
                    {
                        subtotal = subtotal + Convert.ToDouble(et.Text);
                    }
                }
            }
            return subtotal;
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
                    d.ProductID = item["ProductID"];
                    output.Add(d);
                }
                mlist = output;
                var listView = FindViewById<ListView>(Resource.Id.listOrderedProduct);
                productAdapter = new OrderedProductAdapter(this, mlist);
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