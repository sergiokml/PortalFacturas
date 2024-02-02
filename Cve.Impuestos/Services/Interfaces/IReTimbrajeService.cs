namespace Cve.Impuestos.Services.Interfaces
{
    public interface IReTimbrajeService
    {
        Task<IReTimbrajeService> Conectar();
        Task<IReTimbrajeService> SolicitaFolios(
            string rut,
            string dv,
            string tipo,
            CancellationToken token
        );
        Task<IReTimbrajeService> SeleccionaFolios(CancellationToken token);
        Task<IReTimbrajeService> GeneraFolios(CancellationToken token);
        Task<string> DescargaFolio(CancellationToken token);
    }
}
