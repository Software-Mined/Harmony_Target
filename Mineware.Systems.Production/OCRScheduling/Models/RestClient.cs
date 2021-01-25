using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mineware.Systems.Production.OCRScheduling.Models
{
    public class ClientConnect
    {
        private HttpClient httpClient;

        private const string BaseUrl = "http://10.148.225.119:909/";

        public ClientConnect()
        {
            httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));
            httpClient.BaseAddress = new Uri(BaseUrl);
        }


        public HttpResponseMessage Get(String path)
        {
            HttpResponseMessage response = httpClient.GetAsync(path).Result;
            return response;
        }

        public async Task<string> GetWithParameters(String path, Dictionary<string, string> urlParameters)
        {
            String parameters = BuildURLParametersString(urlParameters);
            HttpResponseMessage response = httpClient.GetAsync(path + parameters).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public HttpResponseMessage GetWithCustomHeaders(String path, Dictionary<string, string> headers)
        {
            AddHeaders(headers);
            HttpResponseMessage response = httpClient.GetAsync(path).Result;
            return response;
        }

        public async Task<string> GetWithCustomHeadersAndParameters(String path, Dictionary<string, string> headers,
        Dictionary<string, string> urlParameters)
        {
            AddHeaders(headers);

            String parameters = BuildURLParametersString(urlParameters);

            HttpResponseMessage response = httpClient.GetAsync(path + parameters).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public string PostWithBodyAndParameters(String path, Dictionary<string, string> urlParameters, string body = null)
        {
            String parameters = BuildURLParametersString(urlParameters);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(path + parameters, content).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseMessage Post(String path, String body = null)
        {
            HttpResponseMessage response = httpClient.PostAsync(path, new StringContent(body)).Result;
            return response;
        }

        public HttpResponseMessage Put(String path, String body = null)
        {
            HttpResponseMessage response = httpClient.PutAsync(path, new StringContent(body)).Result;
            return response;
        }

        public HttpResponseMessage Delete(String path)
        {
            HttpResponseMessage response = httpClient.DeleteAsync(path).Result;
            return response;
        }

        private String BuildURLParametersString(Dictionary<string, string> parameters)
        {
            UriBuilder uriBuilder = new UriBuilder();
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var urlParameter in parameters)
            {
                query[urlParameter.Key] = urlParameter.Value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.Query;
        }

        private void AddHeaders(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}