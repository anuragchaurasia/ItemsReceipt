using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DataLayer.Helper
{
    public class PushHelper
    {
        public PushHelper()
        {

        }

        public static bool SendPushMessage(PushNotificationData PushDetail)
        {
            return new PushHelper().SendPushMessageInternal(PushDetail);
        }

        private bool SendPushMessageInternal(PushNotificationData PushData)
        {
            switch (PushData.DeviceType)
            {
                case (DeviceType.Android):
                    {
                        SendPushToAndriod(PushData);
                    }
                    break;

                case (DeviceType.IPhone):
                    {
                        //SendPushToIoS(PushData);
                    }
                    break;

                case (DeviceType.WindowsPhone):
                    {
                        //To Do Send Push To Window phone
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// sending push to Android
        /// </summary>
        /// <param name="deviceRegistrationId"></param>
        /// <param name="pushNotifyType"></param>
        /// <param name="pushInformation"></param>
        private void SendPushToAndriod(PushNotificationData pushData)
        {
            //string pushMessage = "{ \"payload\": {\"message\":\"" + pushData.Message + "\",\"badge\":1,\"sound\":\"default\",\"PushType\":\"" + "Instant" + "\"," +"" +"}}";//pushData.GetJsonFromCustomItems()
            //string pushMessage = "{\"payload\":\""+ pushData.Message + "\"}";//pushData.Message;
            //Fluent construction of an Android GCM Notification
            //IMPORTANT: For Android you MUST use your own RegistrationId here that gets generated within your Android app itself!            
            //m_broker.QueueNotification(new GcmNotification().ForDeviceRegistrationId(pushData.DevicePushToken).WithJson(pushMessage));

            var value = pushData.Message;
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", ConfigurationManager.AppSettings["FirebaseServerAPIKey"]));
            tRequest.Headers.Add(string.Format("Sender: id={0}", ConfigurationManager.AppSettings["FirebaseSenderKey"]));
            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + pushData.DevicePushToken + "";

            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}
