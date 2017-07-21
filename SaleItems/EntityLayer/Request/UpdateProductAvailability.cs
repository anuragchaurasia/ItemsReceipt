using EntityLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityLayer.Request
{
    [DataContract]
    public class UpdateProductAvailabilityRequest :BaseRequest
    {
        [DataMember]
        public List<ProductDTO> productInfo { get; set; }
    }
}
