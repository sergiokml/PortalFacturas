﻿using Microsoft.Extensions.Options;

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
        public readonly AppSettings _options;

        public SharePointService(HttpClient _httpClient, IOptions<AppSettings> _options)
        {
            this._httpClient = _httpClient;
            this._options = _options?.Value;
        }

        private async Task CreateAuthorizedHttpClient()
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>("client_id", _options.ClientId),
                    new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                    new KeyValuePair<string, string>("scope", _options.Scope),
                    new KeyValuePair<string, string>("grant_type", _options.GrantType),
                    new KeyValuePair<string, string>("resource", _options.Resource)
                };
            string requestUrl = $"{_options.TenantId}/oauth2/token";
            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, requestContent);
            if (response.IsSuccessStatusCode)
            {
                Stream responseBody = await response.Content.ReadAsStreamAsync();
                dynamic tokenResponse = await JsonSerializer.DeserializeAsync(
                    responseBody,
                    typeof(TokenSp)
                );
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Authorization",
                    $"Bearer {tokenResponse?.AccessToken}"
                );
            }
            else
            {
                throw new Exception($"Token inválido en Sharepoint.");
            }
        }

        public async Task<byte[]> DownloadConvertedFileAsync(string fileId)
        {
            await CreateAuthorizedHttpClient();

            string path = $"{_options.Resource}beta/sites/{_options.SiteId}/drive/items/";
            string requestUrl = $"{path}{fileId}/content";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                throw new Exception($"No encontrado en Sharepoint.");
            }
        }
    }
}
