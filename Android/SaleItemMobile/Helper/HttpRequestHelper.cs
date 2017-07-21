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
using System.Threading.Tasks;
using System.Json;
using System.Net.Http;

namespace SaleItemMobile.Helper
{
    public static class HttpRequestHelper<T> where T : class
    {
        public static async Task<JsonValue> POSTreq(ServiceTypes services, T saleEntity)
        {
            Uri requestUri = new Uri(SaleItemGlobal.serviceURL + services);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(saleEntity);
            var objClint = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;
            string responJsonText = await respon.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonObject.Parse(responJsonText));
        }

        public static async Task<JsonValue> GetRequest(ServiceTypes services, String URLAppender)
        {
            Uri geturi = new Uri(SaleItemGlobal.serviceURL + services);
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage responseGet = await client.GetAsync(geturi + URLAppender);
            string response = await responseGet.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonObject.Parse(response));
        }
    }
}