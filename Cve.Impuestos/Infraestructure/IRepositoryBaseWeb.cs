namespace Cve.Impuestos.Infraestructure
{
    public interface IRepositoryBaseWeb
    {
        Task<HttpResponseMessage>? SendAsync(string url, string token);
        Task<HttpResponseMessage>? PostApiJson(
            string json,
            string url,
            string token,
            CancellationToken canceltoken
        );
        Task<HttpResponseMessage>? PostFormWeb(
            List<KeyValuePair<string, string>> values,
            CancellationToken token,
            string url = null!
        );
        Task GenerarTokenSesion(string url);
    }
}
