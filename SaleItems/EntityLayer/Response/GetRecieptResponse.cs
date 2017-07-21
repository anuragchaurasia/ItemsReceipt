using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EntityLayer.DTO;

namespace EntityLayer.Response
{
    [DataContract]
    public class GetRecieptResponse :BaseResponse
    {
        [DataMember]
        public List<RecieptDTO> reciepts { get; set; }
    }
}
