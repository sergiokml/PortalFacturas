using System.Xml.Linq;

namespace Cve.Impuestos.Services.Interfaces
{
    public interface IFirmaDteService
    {
        Task<XDocument> EnviarCurl(string rutemisor, string dvemisor, string namefile);
        Task<XDocument> EnviarHttp(string rutemisor, string dvemisor, string namefile);
        Task<string> FirmarTradicional(
            List<Tuple<XDocument, string>> docs,
            string idset,
            string rutemisor,
            string rutreceptor,
            string fecharesol,
            string nroresol,
            string tipodte
        );
        // bool Validar(string xmlpath, string name);
    }
}
