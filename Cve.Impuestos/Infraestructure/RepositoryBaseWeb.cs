using System.Text;

using static System.Net.Mime.MediaTypeNames;

namespace Cve.Impuestos.Infraestructure
{
    internal class RepositoryBaseWeb : IRepositoryBaseWeb
    {
        private const string clientName = "SII_WEB";
        private readonly IHttpClientFactory clientFactory;

        public RepositoryBaseWeb(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage>? SendAsync(string url, string token)
        {
            // PARA BAJAR EL EXCEL SII
            HttpClient httpclient = clientFactory.CreateClient(clientName);
            httpclient!.Timeout = TimeSpan.FromMinutes(10);
            HttpRequestMessage request = new(HttpMethod.Get, url);
            HttpResponseMessage? res = await httpclient!.SendAsync(request);
            return res;
        }

        // POST CON JSON (API)
        public async Task<HttpResponseMessage>? PostApiJson(
            string json,
            string url,
            string token,
            CancellationToken canceltoken
        )
        {
            HttpClient httpclient = clientFactory.CreateClient(clientName);
            httpclient.DefaultRequestHeaders.Add("Cookie", $"TOKEN={token}");
            using StringContent? c = new(json, Encoding.UTF8, Application.Json);
            HttpResponseMessage? res = await httpclient!.PostAsync(url, c, canceltoken);
            return res;
        }

        // POST CON FORM VALUES (SCRAPPING)
        public async Task<HttpResponseMessage>? PostFormWeb(
            List<KeyValuePair<string, string>> values,
            CancellationToken token,
            string url = null!
        )
        {
            HttpClient httpclient = clientFactory.CreateClient(clientName);
            FormUrlEncodedContent requestContent = new(values);
            HttpResponseMessage? res = await httpclient!.PostAsync(url, requestContent, token);
            return res;
        }

        // SET COOKIES
        public async Task GenerarTokenSesion(string url)
        {
            HttpClient httpclient = clientFactory.CreateClient(clientName);
            _ = await httpclient!.GetAsync($"{Properties.Impuestos.UrlTokenSeed}?referencia={url}");
        }
    }
}
