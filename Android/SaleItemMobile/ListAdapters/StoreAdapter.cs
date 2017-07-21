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

namespace SaleItemMobile.ListAdapters
{
    public class StoreAdapter : BaseAdapter<UserDTO>
    {
        public List<UserDTO> uList;
        private Context sContext;
        public StoreAdapter(Context context, List<UserDTO> list)
        {
            uList = list;
            sContext = context;
        }
        public override UserDTO this[int position]
        {
            get
            {
                return uList[position];
            }
        }
        public override int Count
        {
            get
            {
                return uList.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.UserListView, null, false);
                }
                TextView txtStoreName = row.FindViewById<TextView>(Resource.Id.StoreName);
                txtStoreName.Text = uList[position].StoreName;
                TextView txtUserName = row.FindViewById<TextView>(Resource.Id.UserName);
                txtUserName.Text = uList[position].Username;
                TextView txtUserID = row.FindViewById<TextView>(Resource.Id.storeID);
                txtUserID.Text = uList[position].StoreUserID.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally { }
            return row;
        }
    }
}