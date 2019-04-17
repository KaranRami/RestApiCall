using System;
using System.Collections.Generic;
using System.Text;

namespace ApiUtils.DataModel
{
    class UserModel
    {
    }
    
    public class LoginRequestModel : BaseRequestModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
    public class LoginResponseModel : BaseResponseModel
    {
        public string token { get; set; }
    }
}
