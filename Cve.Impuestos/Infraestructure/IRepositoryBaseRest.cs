namespace Cve.Impuestos.Infraestructure
{
    public interface IRepositoryBaseRest
    {
        Task<HttpResponseMessage>? PostJson(string json, string url = null!, string token = null!);
        Task<HttpResponseMessage> SendAsync(
            string token,
            string namefile,
            string rutenvia,
            string dvenvia,
            string rutemisor,
            string dvemisor
        );
        Task<HttpResponseMessage> SendAsync2(
            string token,
            string namefile,
            string rutenvia,
            string dvenvia,
            string rutemisor,
            string dvemisor
        );

        // SOAP
        Task<HttpResponseMessage> SendSoap(
            StringContent content,
            System.Net.Http.HttpMethod method,
            string url,
            string token
        );

        // CURL
        Task<string> ExecuteCurl(string curl);
    }
}
