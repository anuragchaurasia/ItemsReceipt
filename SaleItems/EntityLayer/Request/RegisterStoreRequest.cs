using EntityLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityLayer.Request
{
    [DataContract]
    public class RegisterStoreRequest : BaseRequest
    {
        [DataMember]
        public UserDTO UserInfo { get; set; }
    }
}
