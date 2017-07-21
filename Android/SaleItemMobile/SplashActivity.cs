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
using System.Threading;

namespace SaleItemMobile
{
    [Activity(Label = "Store Communicator", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory =true, Icon ="@drawable/shopping72")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Thread.Sleep(2000);
            StartActivity(typeof(MainActivity));
            // Create your application here
        }
    }
}