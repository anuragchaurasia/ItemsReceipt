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
using Android.Preferences;

namespace SaleItemMobile.Helper
{
    public class SessionHelper
    {
        private ISharedPreferences sharedPreference;
        private ISharedPreferencesEditor sharedPreferenceEditor;
        private Context mContext;

        public SessionHelper(Context context)
        {
            this.mContext = context;
            sharedPreference = PreferenceManager.GetDefaultSharedPreferences(mContext);
            sharedPreferenceEditor = sharedPreference.Edit();
        }

        public void SaveSessionKey(SessionKeys sessionKey, string value)
        {
            sharedPreferenceEditor.PutString(Enum.GetName(typeof(SessionKeys), sessionKey), value);
            sharedPreferenceEditor.Commit();
        }

        public string GetSessionKey(SessionKeys sessionKey)
        {
            return sharedPreference.GetString(Enum.GetName(typeof(SessionKeys), sessionKey), null);
        }
    }
}