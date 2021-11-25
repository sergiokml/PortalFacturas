using PortalFacturas.Models;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortalFacturas.Services
{
    public interface IXslMapperFunctionService
    {
        Task<string> ConvertDocument(string res);
    }

    public class XslMapperFunctionService : IXslMapperFunctionService
    {
        private readonly HttpClient _httpClient;

        public XslMapperFunctionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> ConvertDocument(string res)
        {
            UploadModel uploadModel = new()
            {
                InputXml = res
            };
            uploadModel.Mapper.Directory = "";
            uploadModel.Mapper.Name = "Custodium.xslt";
            HttpContent content2 = new StringContent(JsonSerializer.Serialize(uploadModel), Encoding.UTF8, "application/json");
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Post,
                //RequestUri = new Uri(""),
                Content = content2
            };
            ResponseModel doc = await (await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead)).Content.ReadFromJsonAsync<ResponseModel>();
            if (doc.Content == null)
            {
                throw new Exception("Mensaje = No se pudo acceder a Azure");

            }
            return doc.Content;
        }
    }
}
