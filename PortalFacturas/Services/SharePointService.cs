
using PortalFacturas.Interfaces;
using PortalFacturas.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PortalFacturas.Services
{
    public class SharePointService : ISharePointService
    {
        private HttpClient _httpClient;
        public readonly OptionsModel _options;

        public SharePointService(OptionsModel _options, HttpClient httpClient)
        {
            _httpClient = httpClient;
            this._options = _options;
        }

        private async Task<HttpClient> CreateAuthorizedHttpClient()
        {
            if (_httpClient != null)
            {
                return _httpClient;
            }

            string token = await GetAccessTokenAsync();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            return _httpClient;
        }

        public async Task<byte[]> DownloadConvertedFileAsync(string path, string fileId, string targetFormat)
        {

            HttpClient httpClient = await CreateAuthorizedHttpClient();

            string requestUrl = $"{path}{fileId}/content?format={targetFormat}";
            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                byte[] fileContent = await response.Content.ReadAsByteArrayAsync();
                return fileContent;
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Download of converted file failed with status {response.StatusCode} and message {message}");
            }

        }

        private async Task<string> GetAccessTokenAsync()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                new KeyValuePair<string, string>("scope", _options.Scope),
                new KeyValuePair<string, string>("grant_type", _options.GrantType),
                new KeyValuePair<string, string>("resource", _options.Resource)
            };
            string requestUrl = $"{_options.Endpoint}{_options.TenantId}/oauth2/token";
            // HttpClient client = new HttpClient();

            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, requestContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = System.Text.Json.JsonSerializer.Deserialize<string>(responseBody);
            return tokenResponse?.access_token;
        }
    }
}
