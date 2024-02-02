using Cve.Coordinador.Models;

namespace Cve.Coordinador.Services.Interfaces
{
    public interface IAuxiliaryFileService
    {
        Task<int> PutArchivo(string path, string name, CancellationToken ct);
        Task<CreditorJobResult?> CrearJob(int fileId, int idCen, int idPm, CancellationToken ct);
        Task<CreditorJobResult?> PublicarJob(CreditorJobResult job, CancellationToken ct);
    }
}
