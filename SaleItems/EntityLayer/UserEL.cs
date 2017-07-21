using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityLayer
{
    public class UserEL
    {
        public int StoreUserID { get; set; }
        public string StoreName { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string DeviceID { get; set; }
        public string AuthToken { get; set; }
        public Nullable<bool> Active { get; set; }
        public string DeviceType { get; set; }
    }
}
