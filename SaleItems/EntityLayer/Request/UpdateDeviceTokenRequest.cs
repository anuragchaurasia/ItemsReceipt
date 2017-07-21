using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityLayer.Request
{
    [DataContract]
    public class UpdateDeviceTokenRequest : BaseRequest
    {

        [DataMember(Order = 2, IsRequired = true)]
        public string DevicePushToken { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public string DeviceType { get; set; }

    }
}
