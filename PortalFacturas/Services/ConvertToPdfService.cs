using PortalFacturas.Models;

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PortalFacturas.Services
{
    public interface IConvertToPdfService
    {
        Task<byte[]> ConvertToPdf(string content, string filename);
    }

    public class ConvertToPdfService : IConvertToPdfService
    {
        private readonly HttpClient _httpClient;

        public ConvertToPdfService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> ConvertToPdf(string content, string filename)
        {
            PdfRequest jsondoc = new PdfRequest
            {
                Html = content,
                Inline = true,
                FileName = $"{filename}.pdf"
            };
            string contentt = JsonSerializer.Serialize(jsondoc);
            HttpRequestMessage request =
                new()
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(contentt, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri("https://v2.api2pdf.com/chrome/pdf/html")
                };
            request.Headers.TryAddWithoutValidation(
                "Authorization",
                "3135c383-6882-408a-a965-2212131d3a48"
            );
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonNode.Parse(body).AsObject();
                //return (string)obj["FileUrl"];

                return await _httpClient.GetByteArrayAsync((string)obj["FileUrl"]);
            }
            else
            {
                throw new Exception($"No se pudo convertir el documento.");
            }
        }
    }
}
