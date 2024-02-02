using Cve.Impuestos.Services.Interfaces;

namespace Cve.Impuestos.Extensions
{
    public static class TimbrajeExtension
    {
        public static async Task<ITimbrajeService> SolicitaFolios(
            this Task<ITimbrajeService> helper,
            string rut,
            string dv,
            string tipo,
            string afecto,
            CancellationToken token
        )
        {
            ITimbrajeService instance = await helper;
            return await instance.SolicitaFolios(rut, dv, tipo, afecto, token);
        }

        public static async Task<ITimbrajeService> SolicitaFolios(
            this Task<ITimbrajeService> helper,
            string rut,
            string dv,
            string tipo,
            CancellationToken token
        )
        {
            ITimbrajeService instance = await helper;
            return await instance.SolicitaFolios(rut, dv, tipo, null!, token);
        }

        public static async Task<ITimbrajeService> SeleccionaFolios(
            this Task<ITimbrajeService> helper,
            int cantsolicitada,
            string tipo,
            CancellationToken token
        )
        {
            ITimbrajeService instance = await helper;
            return await instance.SeleccionaFolios(cantsolicitada, tipo, token);
        }

        public static async Task<ITimbrajeService> GeneraFolios(
            this Task<ITimbrajeService> helper,
            CancellationToken token
        )
        {
            ITimbrajeService instance = await helper;
            return await instance.GeneraFolios(token);
        }

        public static async Task<string> DescargaFolio(
            this Task<ITimbrajeService> helper,
            CancellationToken token
        )
        {
            ITimbrajeService instance = await helper;
            return await instance.DescargaFolio(token);
        }
    }
}
