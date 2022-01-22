using Microsoft.Extensions.Options;

using PortalFacturas.Models;

using System;
using System.Collections.Generic;
using System.IO;
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
            this._options = _options?.Value;
        }

        private async Task CreateAuthorizedHttpClient()
        {
            // GUARDAR EL TOKEN EN UNA VARIABLE DE SESSION PARA NO VENIR EN CADA DOC.
          
                string requestUrl = string.Empty;
                HttpResponseMessage response = new HttpResponseMessage();
                Stream responseBody = null;
                try
                {
                    List<KeyValuePair<string, string>> values = new()
                    {
                        new KeyValuePair<string, string>("client_id", _options.ClientId),
                        new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                        new KeyValuePair<string, string>("scope", _options.Scope),
                        new KeyValuePair<string, string>("grant_type", _options.GrantType),
                        new KeyValuePair<string, string>("resource", _options.Resource)
                    };
                    requestUrl = $"{_options.TenantId}/oauth2/token";
                    FormUrlEncodedContent requestContent = new FormUrlEncodedContent(values);
                    response = await _httpClient.PostAsync(requestUrl, requestContent);
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStreamAsync();
                        dynamic tokenResponse = await JsonSerializer.DeserializeAsync(responseBody, typeof(TokenSp));
                        //return tokenResponse?.AccessToken;
                        //_httpClient.DefaultRequestHeaders.Clear();
                        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResponse?.AccessToken}");

                    }
                    else
                    {
                        throw new Exception($"{response.StatusCode}---{responseBody}*****{_httpClient.BaseAddress}_{requestUrl}");
                    }
                }
                catch (Exception)
                {
                    responseBody = await response.Content.ReadAsStreamAsync();
                    throw new Exception($"{response.StatusCode}---{responseBody}*****{_httpClient.BaseAddress}_{requestUrl}");
                }
                //return null;

        }

        public async Task<byte[]> DownloadConvertedFileAsync(string fileId)
        {
            await CreateAuthorizedHttpClient();
            try
            {
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
                    throw new Exception($"{requestUrl}---------{response.StatusCode}--{message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"DownloadConvertedFileAsync {ex.Message}");
            }


        }


    }
}
