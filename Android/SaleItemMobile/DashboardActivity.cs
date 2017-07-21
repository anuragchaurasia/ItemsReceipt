using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;

namespace SaleItemMobile
{
    [Activity(Label = "DashboardActivity", Theme = "@style/Theme.DesignDemo")]
    public class DashboardActivity : Activity
    {
        protected DrawerLayout drawerLayout;
        NavigationView navigationView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Dashboard);
            //var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowTitleEnabled(false);
            //SupportActionBar.SetHomeButtonEnabled(true);
            //SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.Add);
            //drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            //var dashboardActivity = new Intent(this, typeof(LoginActivity));
            //StartActivity(dashboardActivity);

            Button btnPrepareOrder = FindViewById<Button>(Resource.Id.btnePrepareOrder);
            Button btnViewOrder = FindViewById<Button>(Resource.Id.btnViewOrder);
            Button btnViewQuote = FindViewById<Button>(Resource.Id.btnViewQuotes);

            btnPrepareOrder.Click += delegate
            {
                var recieptActivity = new Intent(this, typeof(ReceiptActivity));
                StartActivity(recieptActivity);
            };

            btnViewOrder.Click += delegate
            {
                var orderRecieptActivity = new Intent(this, typeof(OrderRecieptActivity));
                StartActivity(orderRecieptActivity);
            };

            btnViewQuote.Click += delegate
            {
                var quoteRecieptActivity = new Intent(this, typeof(QuotedRecieptActivity));
                StartActivity(quoteRecieptActivity);
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}