using Cve.Impuestos.Models;

namespace Cve.Impuestos.Services.Interfaces
{
    public interface IMisDteService
    {
        Task<ResResumen?> GetResumen(MetaData meta, Data data);
        Task<ResDetalle?> GetDetalle(
            MetaData meta,
            Data data,
            string url,
            CancellationToken canceltoken
        );
        Task<DescargaCsv?> DescargarCsv(Data data, string ns, string url);
    }
}
