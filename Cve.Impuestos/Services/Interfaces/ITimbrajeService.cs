namespace Cve.Impuestos.Services.Interfaces
{
    public interface ITimbrajeService
    {
        Task<ITimbrajeService> Conectar();
        Task<ITimbrajeService> SolicitaFolios(
            string rut,
            string dv,
            string tipo,
            string afectoiva,
            CancellationToken token
        );
        Task<ITimbrajeService> SeleccionaFolios(
            int cantsolicitada,
            string tipo,
            CancellationToken token
        );
        Task<ITimbrajeService> GeneraFolios(CancellationToken token);
        Task<string> DescargaFolio(CancellationToken token);
        string CantPermitida { get; }
        string CantPermitidaSinUsar { get; }
        string MensajeError { get; }
    }
}
