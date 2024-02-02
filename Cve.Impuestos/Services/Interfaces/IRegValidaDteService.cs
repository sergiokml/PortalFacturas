using Cve.Impuestos.Models;
using Cve.Impuestos.Serializadores;

using static Cve.Impuestos.Serializadores.RespListarEventosHistDoc;

namespace Cve.Impuestos.Services.Interfaces
{
    public interface IRegValidaDteService
    {
        Task<RegValidaDteResp?> ConsultaEstadoDte(
            string rutemisor,
            string dv,
            string folio,
            string tipo
        );

        Task<RESPUESTA> ConsultaEstadoDteAv(
            string RutEmpresa,
            string DvEmpresa,
            string RutReceptor,
            string DvReceptor,
            string TipoDte,
            string FolioDte,
            string FechaEmisionDte,
            string MontoDte,
            string FirmaDte
        );

        // SOAP
        Task<@return?> listarEventosHistDoc(
            string rutEmisor,
            string dvEmisor,
            string tipoDoc,
            string folio
        );
        Task<string?> ConsultarFechaRecepcionSii(
            string rutEmisor,
            string dvEmisor,
            string tipoDoc,
            string folio
        );
        Task<RespIngresarAceptacionReclamoDoc.@return> IngresaAceptacionReclamo(
            string rutEmisor,
            string dvEmisor,
            string tipoDoc,
            string folio,
            string accionDoc
        );
    }
}
