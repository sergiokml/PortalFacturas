using Microsoft.Extensions.Options;

using PortalFacturas.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortalFacturas.Services
{
    public interface ISharePointService
    {
        Task<byte[]> DownloadConvertedFileAsync(string fileId);
    }


    public class SharePointService : ISharePointService
    {
        private readonly HttpClient _httpClient;
        public readonly OptionsModel _options;

        public SharePointService(HttpClient _httpClient, IOptions<OptionsModel> _options)
        {
            this._httpClient = _httpClient;
            this._options = _options.Value;
        }

        private async Task CreateAuthorizedHttpClient()
        {
            //if (_httpClient != null)
            //{
            //    return _httpClient;
            //}

            string token = await GetAccessTokenAsync();
            // _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            //return _httpClient;
        }

        public async Task<byte[]> DownloadConvertedFileAsync(string fileId)
        {

            await CreateAuthorizedHttpClient();

            string path = $"{_options.Resource}beta/sites/{_options.SiteId}/drive/items/";
            string requestUrl = $"{path}{fileId}/content";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
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
            List<KeyValuePair<string, string>> values = new()
            {
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                new KeyValuePair<string, string>("scope", _options.Scope),
                new KeyValuePair<string, string>("grant_type", _options.GrantType),
                new KeyValuePair<string, string>("resource", _options.Resource)
            };
            string requestUrl = $"{_options.TenantId}/oauth2/token";
            // HttpClient client = new HttpClient();

            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, requestContent);



            //ResponseToken r = await (await _httpClient.PostAsJsonAsync(requestUrl, requestContent))
            //    .Content.ReadFromJsonAsync<ResponseToken>();

            System.IO.Stream responseBody = await response.Content.ReadAsStreamAsync();
            dynamic tokenResponse = await JsonSerializer.DeserializeAsync(responseBody, typeof(ResponseToken));
            return tokenResponse?.AccessToken;
        }
    }
}
