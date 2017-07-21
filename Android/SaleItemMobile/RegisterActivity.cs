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
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using SaleItemMobile.SaleEntities;
using System.Net.Http.Headers;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using SaleItemMobile.Helper;
using SaleItemMobile.DTO;
using Android.Gms.Common;
using SaleItemMobile.PushHelper;
using Firebase.Iid;

namespace SaleItemMobile
{
    [Activity(Label = "Register Store")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);
            EditText storeNameText = FindViewById<EditText>(Resource.Id.editTextStoreName);
            EditText storeUsernameText = FindViewById<EditText>(Resource.Id.editTextUserName);
            EditText storeEmailText = FindViewById<EditText>(Resource.Id.editTextEmail);
            EditText storeAddressText = FindViewById<EditText>(Resource.Id.editTextAddress);
            EditText storePasswordText = FindViewById<EditText>(Resource.Id.textViewPassword);
            Button btnRegisterStore = FindViewById<Button>(Resource.Id.button1);
            Firebase.FirebaseApp.InitializeApp(this);
            FirebaseIIDService fireService = new FirebaseIIDService();
            
            btnRegisterStore.Click += async (sender, e) =>
                {
                    try
                    {
                        UserDTO userinfo = new UserDTO();
                        userinfo.Active = true;
                        userinfo.Address = storeAddressText.Text;
                        userinfo.Email = storeEmailText.Text;
                        userinfo.Password = storePasswordText.Text;
                        userinfo.StoreName = storeNameText.Text;
                        userinfo.Username = storeUsernameText.Text;
                        RegisterEntity registerEntity = new RegisterEntity { AuthToken = "", UserInfo = userinfo };
                        JsonValue json = await HttpRequestHelper<RegisterEntity>.POSTreq(ServiceTypes.RegisterStore, registerEntity);
                        ParseJSON(json);
                    }
                    catch (Exception ex)
                    {

                    }
                };
        }

        private void ParseJSON(JsonValue json)
        {
            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue registrationResults = json;

            // Extract the "stationName" (location string) and write it to the location TextBox:
            bool isSuccess = registrationResults["IsSuccess"];
            int UserID = registrationResults["UserID"];

            if (isSuccess)
            {
                Toast.MakeText(this, "Registration completed successfully. You can login now.", ToastLength.Short).Show();

            }
            else
            {
                string errorMessage = registrationResults["Message"];
                Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
            }
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    //msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    //msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                //msgText.Text = "Google Play Services is available.";
                return true;
            }
        }

    }
}