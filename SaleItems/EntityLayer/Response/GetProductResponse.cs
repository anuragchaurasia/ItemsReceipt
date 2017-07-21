using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EntityLayer.DTO;

namespace EntityLayer.Response
{
    [DataContract]
    public class GetProductResponse : BaseResponse
    {
        [DataMember]
        public List<ProductDTO> products { get; set; }
    }
}
