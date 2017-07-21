using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityLayer.Request
{
    [DataContract]
    public class AndroidFCMPushNotificationRequest
    {
        [DataMember]
        public string serverApiKey { get; set; }

        [DataMember]
        public string senderId { get; set; }

        [DataMember]
        public string deviceId { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string message { get; set; }
    }
}
