using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using SaleItemMobile.DTO;
using SaleItemMobile.SaleEntities;
using System.Json;
using SaleItemMobile.Helper;
using Xamarin.Forms;

namespace SaleItemMobile.PushHelper
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SessionHelper sessionHelper = new SessionHelper(this);
            sessionHelper.SaveSessionKey(SessionKeys.PUSH_TOKEN_KEY, refreshedToken);
        }
        
    }
}