using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EntityLayer.DTO;

namespace EntityLayer.Request
{
    [DataContract]
    public class AddProductRequest : BaseRequest
    {
        [DataMember]
        public ProductDTO productInfo { get; set; }
    }
}
