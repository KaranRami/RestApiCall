using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ModernHttpClient;

namespace ApiUtils
{
    public class ServiceReaponseHeader
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
        public Exception ErrorException { get; set; }
    }
    public class ServiceRequest
    {
        public string RequestUrl { get; set; }
        public HttpContent Content { get; set; }
        public string AuthToken { get; set; }
        public RequestMethodTypes RequestMethod { get; set; }
    }
    public enum RequestMethodTypes
    {
        Get,
        Post,
        Put,
        Delete,
    }
    public class HttpClientUtils
    {
        public HttpClientUtils()
        {
            throw new Exception("Not for direct use, Instead call GetInstance method.");
        }

        public static HttpClient GetInstance(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new Exception("Base Url can not be null or empty string.");
            if (Instance == null)
                Instance = new HttpClient(new NativeMessageHandler())
                {
                    BaseAddress = new Uri(baseUrl),
                    Timeout = new TimeSpan(0, 0, 30),
                };
            return Instance;
        }
        private static HttpClient Instance { get; set; }
    }
    public class RestApiUtils
    {
        public RestApiUtils(string baseUrl)
        {
            Client = HttpClientUtils.GetInstance(baseUrl);
        }
        private HttpClient Client { get; set; }
        public async Task<ServiceReaponseHeader> MakeServiceCall(ServiceRequest serviceRequest, CancellationToken cancellationToken)
        {
            ServiceReaponseHeader serviceReaponseHeader = new ServiceReaponseHeader();
            try
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess==Xamarin.Essentials.NetworkAccess.Internet)
                {
                    if (!string.IsNullOrEmpty(serviceRequest.AuthToken))
                    {
                        Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", serviceRequest.AuthToken);
                    }
                    HttpResponseMessage response = null;
                    Console.WriteLine("Api Call Started: " + serviceRequest.RequestUrl + " " + DateTime.Now.TimeOfDay);
                    switch (serviceRequest.RequestMethod)
                    {
                        case RequestMethodTypes.Get:
                            response = await Client.GetAsync(serviceRequest.RequestUrl, cancellationToken);
                            break;
                        case RequestMethodTypes.Post:
                            response = await Client.PostAsync(serviceRequest.RequestUrl, serviceRequest.Content, cancellationToken);
                            break;
                        case RequestMethodTypes.Put:
                            response = await Client.PutAsync(serviceRequest.RequestUrl, serviceRequest.Content, cancellationToken);
                            break;
                        case RequestMethodTypes.Delete:
                            response = await Client.DeleteAsync(serviceRequest.RequestUrl, cancellationToken);
                            break;
                    }
                    Console.WriteLine("Api Call Ended: " + serviceRequest.RequestUrl + " " + DateTime.Now.TimeOfDay);
                    serviceReaponseHeader.StatusCode = response.StatusCode;
                    serviceReaponseHeader.Response = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    serviceReaponseHeader.ErrorException = new WebException("No internet connection.");
                }
            }
            catch (Exception ex)
            {
                serviceReaponseHeader.ErrorException = ex;
            }

            return serviceReaponseHeader;
        }
    }
}
