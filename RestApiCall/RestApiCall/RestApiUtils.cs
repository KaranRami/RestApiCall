using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using ModernHttpClient;

namespace RestApiCall
{
    public class ServiceReaponseHeader
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class RestApiUtils
    {
        public RestApiUtils()
        {
            throw new Exception("Not for direct use, Instead call GetInstance method.");
        }
        public RestApiUtils(string baseUrl)
        {
            Client = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress = new Uri(baseUrl),
                Timeout=new TimeSpan(0,0,30),
            };
            this.Ping = new Ping();
        }
        /// <summary>
        /// Returns instance of service call to make rest api calls.
        /// </summary>
        /// <returns>RestApiUtils</returns>
        /// <param name="baseUrl">Base URL for Rest services</param>
        public static RestApiUtils GetInstance(string baseUrl)
        {
            if(string.IsNullOrEmpty(baseUrl))
                throw new Exception("Base Url can not be null or empty string.");
            if (Instance==null)
                Instance= new RestApiUtils(baseUrl);
            return Instance;
        }
        private static RestApiUtils Instance { get; set; }
        private HttpClient Client { get; set; }
        private Ping Ping { get; set; }

        public async Task<ServiceReaponseHeader> MakeServiceCall(ServiceRequest serviceRequest,CancellationToken cancellationToken)
        {
            ServiceReaponseHeader serviceReaponseHeader = new ServiceReaponseHeader();
            PingReply reply = this.Ping.Send("google.com", 30000);
            if(reply.Status == IPStatus.Success)
            {
                //Client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));
                try
                {
                    Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", serviceRequest.AuthToken);
                    HttpResponseMessage response = null;
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
                    serviceReaponseHeader.StatusCode = response.StatusCode;
                    serviceReaponseHeader.Response = await response.Content.ReadAsStringAsync();
                }
                catch(Exception ex)
                {
                    serviceReaponseHeader.ErrorException = ex;
                }
            }
            else
            {
                serviceReaponseHeader.ErrorException = new WebException("No or slow internet connection.");
            }
            return serviceReaponseHeader;
        }
        public async Task<ServiceReaponseHeader> CallPost(ServiceRequest serviceRequest,CancellationToken cancellationToken)
        {
            ServiceReaponseHeader serviceReaponseHeader = new ServiceReaponseHeader();
            PingReply reply = this.Ping.Send("google.com", 30000);
            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", serviceRequest.AuthToken);
                    HttpResponseMessage response = await Client.PostAsync(serviceRequest.RequestUrl, serviceRequest.Content, cancellationToken);
                    serviceReaponseHeader.StatusCode = response.StatusCode;
                    serviceReaponseHeader.Response = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    serviceReaponseHeader.ErrorException = ex;
                }
            }
            else
            {
                serviceReaponseHeader.ErrorException = new WebException("No or slow internet connection.");
            }
            return serviceReaponseHeader;
        }
        public async Task<ServiceReaponseHeader> CallPut(ServiceRequest serviceRequest,CancellationToken cancellationToken)
        {
            ServiceReaponseHeader serviceReaponseHeader = new ServiceReaponseHeader();
            PingReply reply = this.Ping.Send("google.com", 30000);
            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", serviceRequest.AuthToken);
                    HttpResponseMessage response = await Client.PutAsync(serviceRequest.RequestUrl, serviceRequest.Content, cancellationToken);
                    serviceReaponseHeader.StatusCode = response.StatusCode;
                    serviceReaponseHeader.Response = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    serviceReaponseHeader.ErrorException = ex;
                }
            }
            else
            {
                serviceReaponseHeader.ErrorException = new WebException("No or slow internet connection.");
            }
            return serviceReaponseHeader;
        }
        public async Task<ServiceReaponseHeader> CallDelete(ServiceRequest serviceRequest,CancellationToken cancellationToken)
        {
            ServiceReaponseHeader serviceReaponseHeader = new ServiceReaponseHeader();
            PingReply reply = this.Ping.Send("google.com", 30000);
            if (reply.Status == IPStatus.Success)
            {
                try
                {
                    Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", serviceRequest.AuthToken);
                    HttpResponseMessage response = await Client.DeleteAsync(serviceRequest.RequestUrl, cancellationToken);
                    serviceReaponseHeader.StatusCode = response.StatusCode;
                    serviceReaponseHeader.Response = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    serviceReaponseHeader.ErrorException = ex;
                }
            }
            else
            {
                serviceReaponseHeader.ErrorException = new WebException("No or slow internet connection.");
            }
            return serviceReaponseHeader;
        }
    }
}
