using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Monad;
using Newtonsoft.Json;
using ToastCloud.ObjectStorage.Constants;

namespace ToastCloud.ObjectStorage.HttpRequest
{
    internal class RestClient : IRestClient
    {
        private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();

        public void AddHeader(string key, string value)
        {
            _headers.Add(key, value);
        }

        public async Task<Try<RestResponse<TResponseBody>>> PostAsync<TRequestBody, TResponseBody>(string url, TRequestBody request)
        {
            try
            {
                var requestJson = JsonConvert.SerializeObject(request);
                var requestContent = new StringContent(requestJson);
                requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(HttpConstants.ApplicationJson);

                var client = new HttpClient();
                foreach (var header in _headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                var httpResponse = await client.PostAsync(url, requestContent);
                var httpResponseJson = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponseBody>(httpResponseJson);
                return () => new RestResponse<TResponseBody>(httpResponse.StatusCode, response);
            }
            catch (Exception exception)
            {
                return () => new TryResult<RestResponse<TResponseBody>>(exception);
            }
        }

        public async Task<Try<HttpStatusCode>> PutStreamAsync(string url, Stream stream)
        {
            try
            {
                var requestContent = new StreamContent(stream);

                var client = new HttpClient();
                foreach (var header in _headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                var httpResponse = await client.PutAsync(url, requestContent);
                return () => httpResponse.StatusCode;
            }
            catch (Exception exception)
            {
                return () => new TryResult<HttpStatusCode>(exception);
            }
        }

        public async Task<Try<HttpStatusCode>> DeleteAsync(string url)
        {
            try
            {
                var client = new HttpClient();
                foreach (var header in _headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                var httpResponse = await client.DeleteAsync(url);
                return () => httpResponse.StatusCode;
            }
            catch (Exception exception)
            {
                return () => new TryResult<HttpStatusCode>(exception);
            }
        }

        public async Task<Try<RestResponse<TResponseBody>>> GetAsync<TResponseBody>(string url, params (string key, string value)[] querys)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    MediaTypeWithQualityHeaderValue.Parse(HttpConstants.ApplicationJson));
                foreach (var header in _headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                var httpResponse = await client.GetAsync(UrlUtil.UrlWithQueryString(url, querys));
                var httpResponseJson = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponseBody>(httpResponseJson);
                return () => new RestResponse<TResponseBody>(httpResponse.StatusCode, response);
            }
            catch (Exception exception)
            {
                return () => new TryResult<RestResponse<TResponseBody>>(exception);
            }
        }
    }
}
