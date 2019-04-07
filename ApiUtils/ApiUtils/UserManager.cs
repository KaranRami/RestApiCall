using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiUtils
{
    public class UserManager
    {
        private static UserManager instance;
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new UserManager();
                return instance;
            }
        }

        public async Task<(ServiceReaponseHeader ServiceReaponseHeader, LoginResponseModel Result)> Login(LoginRequestModel loginRequest, CancellationToken cancellationToken)
        {
            ServiceReaponseHeader serviceReaponseHeader = new ServiceReaponseHeader();
            LoginResponseModel result = new LoginResponseModel();
            RestApiUtils restApiUtils = new RestApiUtils(Constants.BaseUrl);
            ServiceRequest serviceRequest = new ServiceRequest()
            {
                Content = new StringContent(loginRequest.Serialize(), Encoding.UTF8, "application/json"),
                RequestMethod = RequestMethodTypes.Post,
                RequestUrl = Constants.LoginUrl,
            };
            serviceReaponseHeader = await restApiUtils.MakeServiceCall(serviceRequest, cancellationToken);
            try
            {
                if (serviceReaponseHeader.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<LoginResponseModel>(serviceReaponseHeader.Response);
                    result.IsSuccess = true;
                    result.ErrorEcxeption = null;
                }
                else if (serviceReaponseHeader.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorData = JsonConvert.DeserializeObject<CommonResponseModel>(serviceReaponseHeader.Response);
                    result.ErrorMessage = errorData.Message;
                    result.IsSuccess = false;
                    result.ErrorEcxeption = null;
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "Something went wrong.";
                    result.ErrorEcxeption = null;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorEcxeption = ex;
            }
            return (ServiceReaponseHeader: serviceReaponseHeader, Result: result);
        }
    }
}
