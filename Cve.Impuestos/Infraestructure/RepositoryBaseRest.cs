using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

using static System.Net.Mime.MediaTypeNames;

namespace Cve.Impuestos.Infraestructure
{
    public class RepositoryBaseRest : IRepositoryBaseRest
    {
        // SII_REST: NO TIENE NADA ADICIONAL.
        private const string clientName = "SII_REST";
        private readonly IHttpClientFactory clientFactory;

        public RepositoryBaseRest(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage>? PostJson(string json, string url, string token)
        {
            HttpClient httpclient = clientFactory.CreateClient(clientName);
            httpclient!.DefaultRequestHeaders.Add("Cookie", $"TOKEN={token}");
            using StringContent? c = new(json, System.Text.Encoding.UTF8, Application.Json);
            HttpResponseMessage? res = await httpclient!.PostAsync(url, c);
            return res;
        }

        // PARA ENVIAR DTE
        public async Task<HttpResponseMessage> SendAsync(
            string token,
            string namefile,
            string rutenvia,
            string dvenvia,
            string rutemisor,
            string dvemisor
        )
        {
            // PROG 1.0;
            byte[] xmlData = File.ReadAllBytes(@$"{Environment.CurrentDirectory}\{namefile}");
            string xmlStr = Encoding.GetEncoding("iso-8859-1").GetString(xmlData);
            Guid b = Guid.NewGuid();
            using HttpRequestMessage request = new(new System.Net.Http.HttpMethod("POST"), "");
            _ = request.Headers.TryAddWithoutValidation(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; PROG 1.0; Win64; x64)"
            );
            _ = request.Headers.TryAddWithoutValidation(
                "Cookie",
                string.Format("TOKEN={0}", token)
            );
            request.Content = new StringContent(
                $@"--{b}
Content-Disposition: form-data; name=""rutSender""

{rutenvia}
--{b}
Content-Disposition: form-data; name=""dvSender""

{dvenvia}
--{b}
Content-Disposition: form-data; name=""rutCompany""

{rutemisor}
--{b}
Content-Disposition: form-data; name=""dvCompany""

{dvemisor}
--{b}
Content-Disposition: form-data; name=""archivo""; filename=""{namefile}""
Content-Type: text/xml;

{xmlStr}
--{b}--
",
                encoding: Encoding.GetEncoding("ISO-8859-1")
            );
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(
                $"multipart/form-data; boundary={b}"
            );
            HttpClient httpclient = clientFactory.CreateClient(clientName);
            httpclient.BaseAddress = new Uri("https://palena.sii.cl/cgi_dte/UPL/DTEUpload");
            HttpResponseMessage response = await httpclient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> SendAsync2(
            string token,
            string namefile,
            string rutenvia,
            string dvenvia,
            string rutemisor,
            string dvemisor
        )
        {
            // PROG 1.0; !!!!!!!!!!!!!!!!!!! NO FUNCIONA!!!!!!!!!!!!!!!!!!!!
            byte[] xmlData = File.ReadAllBytes(@$"{Environment.CurrentDirectory}\{namefile}");
            using HttpRequestMessage request =
                new(HttpMethod.Post, "https://palena.sii.cl/cgi_dte/UPL/DTEUpload");
            _ = request.Headers.TryAddWithoutValidation(
                "Cookie",
                string.Format("TOKEN={0}", token)
            );
            _ = request.Headers.TryAddWithoutValidation(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; PROG 1.0; Win64; x64)"
            );
            MultipartFormDataContent form = new();
            form.Add(new StringContent(rutenvia), @"""rutSender""");
            form.Add(new StringContent(dvenvia), @"""dvSender""");
            form.Add(new StringContent(rutemisor), @"""rutCompany""");
            form.Add(new StringContent(dvemisor), @"""dvCompany""");
            StringContent streamContent =
                new(Encoding.GetEncoding("iso-8859-1").GetString(xmlData));
            streamContent.Headers.Remove("Content-Type");
            streamContent.Headers.TryAddWithoutValidation("Content-Type", "text/xml");
            streamContent.Headers.Add(
                "Content-Disposition",
                "form-data; name=\"archivo\"; filename=\"" + Path.GetFileName(namefile) + "\""
            );
            form.Add(streamContent, "archivo", Path.GetFileName(namefile));
            request.Content = form;
            HttpClient httpclient = clientFactory.CreateClient(clientName);
            HttpResponseMessage response = await httpclient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> SendSoap(
            StringContent content,
            System.Net.Http.HttpMethod method,
            string url,
            string token
        )
        {
            HttpClient httpclient = clientFactory.CreateClient(clientName);
            HttpRequestMessage request = new(method, url) { Content = content };
            request.Headers.Add("Cookie", $"TOKEN={token}");
            HttpResponseMessage msg = await httpclient.SendAsync(request);
            return msg;
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
