using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace PortalFacturas.Consola.Services
{
    public interface ISharePointService
    {
        Task RenovarToken();
        Task<string> UploadStreamAsync(string company, string xml, string nomenclatura);
    }

    internal class SharePointService : ISharePointService
    {
        private readonly AppSettings appSettings;
        private readonly HttpClient _httpClient;

        public SharePointService(HttpClient _httpClient, IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings?.Value;
            this._httpClient = _httpClient; // BaseAdress no debe setearse porque este Service usa varios root.
        }

        public async Task RenovarToken()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>("client_id", appSettings.ClientId),
                    new KeyValuePair<string, string>("client_secret", appSettings.ClientSecret),
                    new KeyValuePair<string, string>("scope", appSettings.Scope),
                    new KeyValuePair<string, string>("grant_type", appSettings.GrantType),
                    new KeyValuePair<string, string>("resource", appSettings.Resource)
                };
            string requestUrl =
                $"{appSettings.UrlApiSharePoint}{appSettings.TenantId}/oauth2/token";
            FormUrlEncodedContent requestContent = new(values);
            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, requestContent);
            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonNode.Parse(body).AsObject();
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Authorization",
                    $"Bearer {(string)obj["access_token"]}"
                );
            }
        }

        public async Task<string> UploadStreamAsync(
            string company,
            string content,
            string nomenclatura
        )
        {
            string path = $"{appSettings.UrlGraph}sites/{appSettings.SiteId}/drive/items/";
            string tmpFileName = $"{nomenclatura}.xml";
            string requestUrl = $"{path}root:/{company}/{tmpFileName}:/content";
            StreamContent requestContent = new(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            HttpResponseMessage response = await _httpClient.PutAsync(requestUrl, requestContent);
            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonNode.Parse(body).AsObject();
                //  await Task.Delay(1000);
                return (string)obj["id"];
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonNode.Parse(message).AsObject();
                // await Task.Delay(1000);
                throw new Exception((string)obj["error"]["message"]);
            }
        }
    }
}
