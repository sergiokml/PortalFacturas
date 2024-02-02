using Cve.Impuestos.Models;

namespace Cve.Impuestos.Services.Interfaces
{
    public interface IContribuyenteService
    {
        Task<IContribuyenteService> Conectar();
        Task<IContribuyenteService> ConsultaRut();
        Task<Dictionary<string, string>> DescargaAntecedentes(
            string rut,
            string dv,
            CancellationToken token
        );
        Task<string> DescargaFile();
        Task<IContribuyenteService> GetCaptcha();
        Task<Dictionary<string, string>> SituacionTributariaTerceros(
            string ruttercero,
            string dvtercero,
            CancellationToken token
        );
        CaptchaModel? CaptchaModel { get; set; }
    }
}
