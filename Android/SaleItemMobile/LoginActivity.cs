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
using SaleItemMobile.SaleEntities;
using System.Json;
using SaleItemMobile.Helper;
using SaleItemMobile.PushHelper;
using Android.Support.V7.App;

namespace SaleItemMobile
{
    [Activity(Label = "LoginActivity", Theme = "@style/Theme.DesignDemo")]
    public class LoginActivity : AppCompatActivity
    {
        Android.App.ProgressDialog progress;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            EditText storeUsernameText = FindViewById<EditText>(Resource.Id.editTextUserName);
            EditText storePasswordText = FindViewById<EditText>(Resource.Id.textViewPassword);
            Button btnLoginStore = FindViewById<Button>(Resource.Id.btnLogin);

            btnLoginStore.Click += async (sender, e) =>
            {
                progress = new Android.App.ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetMessage("Loading... Please wait...");
                progress.SetCancelable(false);
                progress.Show();
                try
                {
                    LoginEntity loginEntity = new LoginEntity { AuthToken = "", UserNameOREmail = storeUsernameText.Text, PasswordHash = storePasswordText.Text };
                    JsonValue json = await HttpRequestHelper<LoginEntity>.POSTreq(ServiceTypes.Login, loginEntity);
                    ParseJSON(json);
                    progress.Hide();
                }
                catch (Exception ex)
                {
                    progress.Dismiss();
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
                Toast.MakeText(this, "User logging in.", ToastLength.Short).Show();
                int UserID = loginResults["UserID"];
                string token = loginResults["Token"];
                string name = loginResults["FullName"];
                string pushToken = loginResults["PushToken"];
                SessionHelper sessionHelper = new SessionHelper(this);
                sessionHelper.SaveSessionKey(SessionKeys.SESSION_TOKEN_KEY, token);
                sessionHelper.SaveSessionKey(SessionKeys.SESSION_USERID_KEY, UserID.ToString());
                sessionHelper.SaveSessionKey(SessionKeys.SESSION_USERNAME_KEY, name);
                string PushToken = sessionHelper.GetSessionKey(SessionKeys.PUSH_TOKEN_KEY);
                
                
                if(!String.IsNullOrEmpty(PushToken))
                {
                    if (!PushToken.Equals(pushToken))
                        SendRegistrationToServer();
                }
                var dashboardActivity = new Intent(this, typeof(DashboardActivity));
                StartActivity(dashboardActivity);
                
            }
            else
            {
                string errorMessage = loginResults["Message"];
                Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
            }
        }

        public async void SendRegistrationToServer()
        {
            SessionHelper sessionHelper = new SessionHelper(this);
            string AuthToken = sessionHelper.GetSessionKey(SessionKeys.SESSION_TOKEN_KEY);
            string PushToken = sessionHelper.GetSessionKey(SessionKeys.PUSH_TOKEN_KEY);
            if (!String.IsNullOrEmpty(AuthToken))
            {
                switch (Xamarin.Forms.Device.OS)
                {
                    case Xamarin.Forms.TargetPlatform.Android:
                        //PushTokenEntities pushEntity = new PushTokenEntities { AuthToken = AuthToken, DevicePushToken = PushToken, DeviceType = DeviceTypeEnum.ANDROID.ToString() };
                        //JsonValue jsonData = await HttpRequestHelper<PushTokenEntities>.POSTreq(ServiceTypes.UpdatePushToken, pushEntity);
                        //break;
                    case Xamarin.Forms.TargetPlatform.Other:
                        PushTokenEntities pushEntity = new PushTokenEntities { AuthToken = AuthToken, DevicePushToken = PushToken, DeviceType = DeviceTypeEnum.ANDROID.ToString() };
                        JsonValue jsonData = await HttpRequestHelper<PushTokenEntities>.POSTreq(ServiceTypes.UpdatePushToken, pushEntity);
                        break;
                    case Xamarin.Forms.TargetPlatform.iOS:
                        break;
                    case Xamarin.Forms.TargetPlatform.Windows:
                        break;
                }
            }

        }
    }
}