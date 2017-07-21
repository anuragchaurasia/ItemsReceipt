using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Xaml;
using Android.Content;
using Android.Util;
using Firebase.Iid;
using Android.Gms.Common;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.V7.App;

namespace SaleItemMobile
{
    [Activity(Label = "SaleItemMobile", Theme = "@style/Theme.DesignDemo", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource

            SetContentView(Resource.Layout.Main);

            Button btnRegisterStore = FindViewById<Button>(Resource.Id.btnRegister);
            Button btnLoginStore = FindViewById<Button>(Resource.Id.btnLogin);

            btnRegisterStore.Click += delegate
            {
                var registerActivity = new Intent(this, typeof(RegisterActivity));
                //activity2.PutExtra("MyData", "Data from Activity1");
                StartActivity(registerActivity);
            };
            
            btnLoginStore.Click += delegate
            {
                var loginActivity = new Intent(this, typeof(LoginActivity));
                //activity2.PutExtra("MyData", "Data from Activity1");
                StartActivity(loginActivity);
            };

        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {

                }
                    //msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
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

