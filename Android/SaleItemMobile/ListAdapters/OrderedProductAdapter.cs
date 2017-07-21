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
using Android.Text;

namespace SaleItemMobile.ListAdapters
{
    public class OrderedProductAdapter : BaseAdapter<ProductDTO>
    {
        public List<ProductDTO> sList;
        private Context sContext;
        View row;

        public OrderedProductAdapter(Context context, List<ProductDTO> list)
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
            row = convertView;
            
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.OrderedProductListView, null, false);
                }
                TextView txtQuantity = row.FindViewById<TextView>(Resource.Id.productQuantity);
                txtQuantity.Text = sList[position].Quantity;
                TextView txtProductName = row.FindViewById<TextView>(Resource.Id.productName);
                txtProductName.Text = sList[position].Name;
                TextView txtProductID = row.FindViewById<TextView>(Resource.Id.productID);
                txtProductID.Text = sList[position].ProductID.ToString();
                EditText productPriceEdit = row.FindViewById<EditText>(Resource.Id.editProductPrice);


                //productPriceEdit.AddTextChangedListener(new PriceUpdate());


                //productPriceEdit.AfterTextChanged += ProductPriceEdit_AfterTextChanged;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally { }
            return row;
        }

        List<double> subtotalList = new List<double>();

        //private void ProductPriceEdit_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        //{
        //    EditText et;
        //    double subtotal = 0.00;
        //    if (row == null)
        //    {
        //        row = LayoutInflater.From(sContext).Inflate(Resource.Layout.OrderedProductListView, null, false);
        //    }
        //    ListView listview = row.FindViewById<ListView>(Resource.Id.listOrderedProduct);

        //    for (int i = 0; i < listview.ChildCount; i++)
        //    {
        //        et = (EditText)listview.GetChildAt(i).FindViewById(Resource.Id.editProductPrice);
        //        if (et != null)
        //        {
        //            if (String.IsNullOrEmpty(et.Text))
        //            {
        //                subtotal = subtotal + 0.00;
        //            }
        //            else
        //            {
        //                subtotal = subtotal + Convert.ToDouble(et.Text);
        //            }
        //        }
        //    }
        //}


    }

    public class PriceUpdate : Java.Lang.Object, ITextWatcher
    {

        public void AfterTextChanged(IEditable s)
        {

        }

        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {
            //Not implemented
        }

        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            //Not implemented
        }
    }
}