using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityLayer.Request
{
    [DataContract]
    public class UserLoginRequest : BaseRequest
    {
        [DataMember]
        public string UserNameOREmail { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }
    }
}
