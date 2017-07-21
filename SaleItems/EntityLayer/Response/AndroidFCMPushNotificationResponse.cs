using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityLayer.Response
{
    public class AndroidFCMPushNotificationResponse
    {
        public bool Successful
        {
            get;
            set;
        }

        public string Response
        {
            get;
            set;
        }
        public Exception Error
        {
            get;
            set;
        }
    }
}
