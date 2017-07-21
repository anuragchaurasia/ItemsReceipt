using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityLayer.DTO
{
    [DataContract]
    public class UserDTO
    {
        [DataMember]
        public int StoreUserID { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string DeviceID { get; set; }
        [DataMember]
        public Nullable<bool> Active { get; set; }
    }
}
