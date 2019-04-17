using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiUtils.DataModel
{
    class BaseModel
    {
    }
    public class BaseRequestModel
    {
        public string Serialize()
        {
            try
            {
                return JsonConvert.SerializeObject(this);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
    public class BaseResponseModel
    {
        [JsonIgnore]
        public bool IsSuccess { get; set; }
        [JsonIgnore]
        public Exception ErrorEcxeption { get; set; }
        [JsonIgnore]
        public string ErrorMessage { get; set; }
    }
    public class CommonResponseModel:BaseResponseModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
