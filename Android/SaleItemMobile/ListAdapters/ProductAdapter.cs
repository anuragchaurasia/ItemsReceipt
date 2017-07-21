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
    public class ProductAdapter : BaseAdapter<ProductDTO>
    {
        public List<ProductDTO> sList;
        private Context sContext;
        public ProductAdapter(Context context, List<ProductDTO> list)
        {
            sList = list;
            sContext = context;
        }
        public override ProductDTO this[int position]
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
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.ProductListView, null, false);
                }
                TextView txtQuantity = row.FindViewById<TextView>(Resource.Id.productQuantity);
                txtQuantity.Text = sList[position].Quantity;
                TextView txtProductName = row.FindViewById<TextView>(Resource.Id.productName);
                txtProductName.Text = sList[position].Name;
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