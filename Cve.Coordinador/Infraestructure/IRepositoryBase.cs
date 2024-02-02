namespace Cve.Coordinador.Infraestructure;

public interface IRepositoryBase
{
    Task<HttpResponseMessage>? GetJson(string q, CancellationToken ct);
    Task<HttpResponseMessage>? PostJson(string url, string content, CancellationToken ct);
    Task<HttpResponseMessage>? PostForm(
        string url,
        FormUrlEncodedContent content,
        CancellationToken ct,
        string token = default!
    );
    Task<HttpResponseMessage>? PutStream(
        string url,
        StreamContent content,
        string token,
        CancellationToken ct
    );
    Task<string> ExecuteCurl(string curl);
    Task<HttpResponseMessage>? Send(
        string token,
        HttpMethod method,
        string url,
        CancellationToken ct
    );
}
