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
    public class RecieptAdapter : BaseAdapter<RecieptDTO>
    {
        public List<RecieptDTO> sList;
        private Context sContext;
        public RecieptAdapter(Context context, List<RecieptDTO> list)
        {
            sList = list;
            sContext = context;
        }
        public override RecieptDTO this[int position]
        {
            get
            {
                return sList[position];
            }
        }
        public override int Count
        {
            get
            {
                return sList.Count;
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
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.RecieptListView, null, false);
                }
                TextView txtName = row.FindViewById<TextView>(Resource.Id.recieptName);
                txtName.Text = sList[position].Name;
                TextView txtID = row.FindViewById<TextView>(Resource.Id.recieptID);
                txtID.Text = sList[position].RecieptID.ToString();
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