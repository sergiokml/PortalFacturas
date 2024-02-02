using Cve.Impuestos.Services.Interfaces;

namespace Cve.Impuestos.Extensions
{
    public static class ReTimbrajeExtension
    {
        public static async Task<IReTimbrajeService> SolicitaFolios(
            this Task<IReTimbrajeService> helper,
            string rut,
            string dv,
            string tipo,
            CancellationToken token
        )
        {
            IReTimbrajeService instance = await helper;
            return await instance.SolicitaFolios(rut, dv, tipo, token);
        }

        public static async Task<IReTimbrajeService> SeleccionaFolios(
            this Task<IReTimbrajeService> helper,
            CancellationToken token
        )
        {
            IReTimbrajeService instance = await helper;
            return await instance.SeleccionaFolios(token);
        }

        public static async Task<IReTimbrajeService> GeneraFolios(
            this Task<IReTimbrajeService> helper,
            CancellationToken token
        )
        {
            IReTimbrajeService instance = await helper;
            return await instance.GeneraFolios(token);
        }

        public static async Task<string> DescargaFolio(
            this Task<IReTimbrajeService> helper,
            CancellationToken token
        )
        {
            IReTimbrajeService instance = await helper;
            return await instance.DescargaFolio(token);
        }
    }
}
