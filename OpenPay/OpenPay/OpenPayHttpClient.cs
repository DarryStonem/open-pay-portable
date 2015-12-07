using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernHttpClient;
using System.Net.Http;
using System.Net.Http.Headers;
using Openpay.Entities;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using OpenPay;

namespace Openpay
{
    public class OpenpayHttpClient
    {
        private static readonly string api_endpoint = "https://api.openpay.mx/v1/";
        private static readonly string api_endpoint_sandbox = "https://sandbox-api.openpay.mx/v1/";
        private static readonly string user_agent = "Openpay .NET v1";
        private static readonly Encoding encoding = Encoding.UTF8;
        private Boolean _isProduction = false;

        public int TimeoutSeconds { get; set; }

        public String MerchantId { get; internal set; }

        public String APIEndpoint { get; set; }

        public String APIKey { get; set; }

        public enum HttpMethod
        {
            GET, POST, DELETE, PUT
        }

        public bool Production
        {
            get
            {
                return _isProduction;
            }
            set
            {
                APIEndpoint = value ? api_endpoint : api_endpoint_sandbox;
                _isProduction = value;
            }
        }

        public OpenpayHttpClient(string api_key, string merchant_id, bool production = false)
        {
            if (String.IsNullOrEmpty(api_endpoint_sandbox))
                throw new ArgumentNullException("api_key");
            if (String.IsNullOrEmpty(merchant_id))
                throw new ArgumentNullException("merchant_id");
            MerchantId = merchant_id;
            APIKey = api_key;
            TimeoutSeconds = 120;
            Production = production;
        }

        protected virtual HttpClient SetupRequest()
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());
            client.DefaultRequestHeaders.Add("User-Agent", user_agent);

            string authInfo = APIKey + ":";
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
            client.Timeout = TimeSpan.FromSeconds(TimeoutSeconds);
            return client;
        }

        public async Task<T> Post<T>(string endpoint, JsonObject obj)
        {
            var json = await DoRequest(endpoint, HttpMethod.POST, obj.ToJson());
            if (json != null)
                return JsonConvert.DeserializeObject<T>(json);
            else
                return default(T);
        }

        public async void Post<T>(string endpoint)
        {
            await DoRequest(endpoint, HttpMethod.POST, null);
        }

        public async Task<T> Get<T>(string endpoint)
        {
            var json = await DoRequest(endpoint, HttpMethod.GET, null);
            if (json != null)
                return JsonConvert.DeserializeObject<T>(json);
            else
                return default(T);
        }

        public async Task<T> Put<T>(string endpoint, JsonObject obj)
        {
            var json = await DoRequest(endpoint, HttpMethod.PUT, obj.ToJson());
            if (json != null)
                return JsonConvert.DeserializeObject<T>(json);
            else
                return default(T);
        }

        public async void Delete(string endpoint)
        {
            await DoRequest(endpoint, HttpMethod.DELETE, null);
        }

        protected virtual async Task<string> DoRequest(string path, HttpMethod method, string body)
        {
            string result = null;
            string endpoint = APIEndpoint + MerchantId + path;
            HttpResponseMessage response;
            Debug.WriteLine("Request to: " + endpoint);
            Debug.WriteLine("WithBody: " + body);

            try
            {
                using (HttpClient req = SetupRequest())
                {
                    switch (method)
                    {
                        case HttpMethod.GET:
                            response = await req.GetAsync(endpoint);
                            break;
                        case HttpMethod.POST:
                            response = await req.PostAsync(endpoint, new StringContent(body, Encoding.UTF8, "application/json"));
                            break;
                        case HttpMethod.DELETE:
                            response = await req.DeleteAsync(endpoint);
                            break;
                        case HttpMethod.PUT:
                            response = await req.PutAsync(endpoint, new StringContent(body, Encoding.UTF8, "application/json"));
                            break;
                        default:
                            response = null;
                            break;
                    }

                    if (response != null)
                    {
                        result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            Debug.WriteLine("Response passed: " + response.ReasonPhrase + " with Status Code" + response.StatusCode);
                        }
                        else
                        {
                            Debug.WriteLine("Response failure: " + response.ReasonPhrase + " with Status Code" + response.StatusCode);
                            if ((int)response.StatusCode <= 500)
                                throw OpenpayException.GetFromJSON(response.StatusCode, result);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Error Exception: " + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Exception: " + ex.StackTrace);
                throw;
            }

            Debug.WriteLine("Result: " + result);
            return result;
        }
    }
}
