using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityLayer.Request
{
    [DataContract]
    public class AddOrderRecieptRequest : BaseRequest
    {
        [DataMember]
        public int RecieptID { get; set; }
        [DataMember]
        public int SenderStoreID { get; set; }
        [DataMember]
        public string OrderTime { get; set; }
        [DataMember]
        public int ReceiverStoreID { get; set; }
    }
}
