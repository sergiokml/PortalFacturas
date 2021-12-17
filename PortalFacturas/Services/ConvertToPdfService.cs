using PortalFacturas.Models;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortalFacturas.Services
{
    public interface IConvertToPdfService
    {
        Task<byte[]> ConvertToPdf(string content);
    }

    public class ConvertToPdfService : IConvertToPdfService
    {
        private readonly HttpClient _httpClient;

        public ConvertToPdfService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<byte[]> ConvertToPdf(string content)
        {
            var postData = new
            {
                html = content,
                json = "true",
                pdf_page = "Letter"

            };
            string contentt = JsonSerializer.Serialize(postData);
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(contentt, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("x-access-token", "Tc2ENx8UpsBwERyhmCy42t0hfVCxubU4GCpoXUDQ98q0DfHP");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"No se pudo convertir el documento.(restack.io)");
            }
            ResponsePdfRestPackModel pdf = await response.Content
                .ReadFromJsonAsync<ResponsePdfRestPackModel>();

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(pdf.Image);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Instrucción facturada, pero no existe el documento en CEN.");
            }
            return await httpResponseMessage.Content.ReadAsByteArrayAsync();
        }
    }
}
