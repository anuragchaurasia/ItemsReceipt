using EntityLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityLayer.Response
{
    [DataContract]
    public class GetQuotedRecieptResponse : BaseResponse
    {
        [DataMember]
        public List<RecieptDTO> reciepts { get; set; }

        [DataMember]
        public List<UserDTO> users { get; set; }

        [DataMember]
        public string noReceiptMessage { get; set; }

        [DataMember]
        public string noUserMessage { get; set; }
    }
}
