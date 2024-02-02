using Cve.Impuestos.Services.Interfaces;

namespace Cve.Impuestos.Extensions
{
    public static class ContribuyenteExtension
    {
        //private static readonly string captcha = string.Empty;

        /// <summary>
        /// Consulta Contribuyentes Autorizados (email de intercambio DTE)
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static async Task<IContribuyenteService> ConsultaRut(
            this Task<IContribuyenteService> helper
        )
        {
            IContribuyenteService instance = await helper;
            return await instance.ConsultaRut();
        }

        /// <summary>
        /// Resultado Email DTE + Html del Contribuyente consultado
        /// </summary>
        /// <returns></returns>
        public static async Task<Dictionary<string, string>> DescargaAntecedentes(
            this Task<IContribuyenteService> helper,
            string rut,
            string dv,
            CancellationToken token
        )
        {
            IContribuyenteService instance = await helper;
            return await instance.DescargaAntecedentes(rut, dv, token);
        }

        /// <summary>
        /// Archivo CVS de Contribuyentes Autorizados (email de intercambio DTE)
        /// </summary>
        /// <returns></returns>
        public static async Task<string> DescargaFile(this Task<IContribuyenteService> helper)
        {
            IContribuyenteService instance = await helper;
            return await instance.DescargaFile();
        }

        /// <summary>
        /// Obtiene lista de Giros
        /// </summary>
        /// <returns></returns>
        public static async Task<Dictionary<string, string>> SituacionTributariaTerceros(
            this Task<IContribuyenteService> helper,
            string ruttercero,
            string dvtercero,
            CancellationToken token
        )
        {
            IContribuyenteService instance = await helper;
            return await instance.SituacionTributariaTerceros(ruttercero, dvtercero, token);
        }

        /// <summary>
        /// Proceso que piden algunos endpoints
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static async Task<IContribuyenteService> GetCaptcha(
            this Task<IContribuyenteService> helper
        )
        {
            IContribuyenteService instance = await helper;
            return await instance.GetCaptcha();
        }

        public static async Task<
            List<Tuple<string, Dictionary<string, string>>>
        > SituacionTributariaTerceros(
            this Task<IContribuyenteService> helper,
            List<Tuple<string, string>> list,
            CancellationToken token
        )
        {
            List<Tuple<string, Dictionary<string, string>>> result = new();
            List<Task<List<Tuple<string, Dictionary<string, string>>>>> tareas = new();
            IContribuyenteService instance = await helper;
            tareas = list!
                .Select(async b =>
                {
                    try
                    {
                        Dictionary<string, string>? res =
                            await instance.SituacionTributariaTerceros(b.Item1, b.Item2!, token);
                        result.Add(new(b.Item1, res));
                    }
                    catch (HttpRequestException)
                    {
                        result.Add(new(b.Item1, null!));
                    }
                    //await Task.Delay(2000);
                    return result;
                })
                .ToList();
            _ = await Task.WhenAll(tareas);
            return result;
        }
    }
}
