namespace Cve.Coordinador.Services.Interfaces
{
    public interface IAuthenticateService
    {
        Task<string> Authenticate(CancellationToken ct);
    }
}
