using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EntityLayer.DTO;

namespace EntityLayer.Request
{
    [DataContract]
    public class AddRecieptRequest : BaseRequest
    {
        [DataMember]
        public RecieptDTO recieptDTO { get; set; }
    }
}
