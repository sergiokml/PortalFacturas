using System.Diagnostics;
using System.Text;

namespace Cve.Coordinador.Infraestructure
{
    internal class RepositoryBase : IRepositoryBase
    {
        private const string clientName = "CEN";
        private readonly IHttpClientFactory clientFactory;

        public RepositoryBase(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage>? GetJson(string url, CancellationToken canceltkn)
        {
            HttpClient httpClient = clientFactory.CreateClient(clientName);
            HttpResponseMessage? res = await httpClient!.GetAsync(url, canceltkn);
            return res;
        }

        public async Task<HttpResponseMessage>? PostJson(
            string url,
            string content,
            CancellationToken canceltkn
        )
        {
            HttpClient httpClient = clientFactory.CreateClient(clientName);
            using StringContent? c = new(content, Encoding.UTF8, "application/json");
            HttpResponseMessage? res = await httpClient!.PostAsync(url, c, canceltkn);
            return res;
        }

        public async Task<HttpResponseMessage>? PostForm(
            string url,
            FormUrlEncodedContent content,
            CancellationToken canceltkn,
            string token = null!
        )
        {
            HttpClient httpClient = clientFactory.CreateClient(clientName);
            _ = httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "Authorization",
                $"Token {token}"
            );
            HttpResponseMessage? res = await httpClient!.PostAsync(url, content, canceltkn);
            return res;
        }

        public async Task<HttpResponseMessage>? PutStream(
            string url,
            StreamContent content,
            string token,
            CancellationToken canceltkn
        )
        {
            HttpClient httpClient = clientFactory.CreateClient(clientName);
            _ = httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "Authorization",
                $"Token {token}"
            );
            HttpResponseMessage? res = await httpClient!.PutAsync(url, content, canceltkn);
            return res;
        }

        public async Task<HttpResponseMessage>? Send(
            string token,
            HttpMethod method,
            string url,
            CancellationToken canceltkn
        )
        {
            HttpClient httpClient = clientFactory.CreateClient(clientName);
            _ = httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "Authorization",
                $"Token {token}"
            );
            HttpRequestMessage request = new(method, $"{url}");
            HttpResponseMessage? res = await httpClient!.SendAsync(request, canceltkn);
            return res;
        }

        public async Task<string> ExecuteCurl(string curl)
        {
            using Process compiler = new();
            compiler.StartInfo = new ProcessStartInfo(
                Path.Combine(Environment.SystemDirectory, "curl.exe"),
                curl
            )
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            _ = compiler.Start();
            string rr = await compiler.StandardOutput.ReadToEndAsync();
            await compiler.WaitForExitAsync();
            return rr;
        }
    }
}
