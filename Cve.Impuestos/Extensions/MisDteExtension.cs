using Cve.Impuestos.Models;
using Cve.Impuestos.Services;
using Cve.Impuestos.Services.Interfaces;

namespace Cve.Impuestos.Extensions
{
    public static class MisDteExtension
    {
        private const string nnssResumen =
            "cl.sii.sdi.lob.diii.consdcv.data.api.interfaces.FacadeService/getResumen";
        private const string nnssventa =
            "cl.sii.sdi.lob.diii.consdcv.data.api.interfaces.FacadeService/getDetalleVenta";
        private const string nnsscompra =
            "cl.sii.sdi.lob.diii.consdcv.data.api.interfaces.FacadeService/getDetalleCompra";

        public static async Task<IEnumerable<ResDetalle>> GetCompras(
            this IMisDteService service,
            string rut,
            string dv,
            string periodo,
            string estadoContab,
            CancellationToken canceltoken
        )
        {
            MetaData metaData = new() { Namespace = nnssResumen, TransactionId = "0" };
            Data data =
                new()
                {
                    RutEmisor = rut,
                    DvEmisor = dv,
                    EstadoContab = estadoContab,
                    Operacion = "COMPRA",
                    Ptributario = Convert.ToDateTime(periodo).ToString("yyyyMM")
                };
            ResResumen? resumen = await service.GetResumen(metaData, data);
            if (resumen != null && resumen.Data != null)
            {
                List<ResDetalle> lista = new();
                List<Task<List<ResDetalle>>> tareas = new();
                tareas = resumen.Data
                    .Select(async b =>
                    {
                        metaData = new() { Namespace = nnsscompra, TransactionId = "0" };
                        data = new()
                        {
                            CodTipoDoc = b.RsmnTipoDocInteger.ToString(),
                            RutEmisor = rut,
                            DvEmisor = dv,
                            EstadoContab = estadoContab,
                            Operacion = "COMPRA",
                            Ptributario = Convert.ToDateTime(periodo).ToString("yyyyMM")
                        };
                        ResDetalle? detalle = await service.GetDetalle(
                            metaData,
                            data,
                            MisDteService.UrlDteRecibidos!,
                            canceltoken
                        );
                        // EN COMPRAS NO TRAE ESTE DATO
                        foreach (Datum item in detalle!.Data!)
                        {
                            item.DetTipoDoc = b.RsmnTipoDocInteger;
                        }
                        lista.Add(detalle!);
                        return lista;
                    })
                    .ToList();
                _ = await Task.WhenAll(tareas);
                return lista;
            }
            return null!;
        }

        public static async Task<IEnumerable<ResDetalle>> GetVentas(
            this IMisDteService service,
            string rut,
            string dv,
            string periodo,
            CancellationToken canceltoken
        )
        {
            MetaData metaData = new() { Namespace = nnssResumen, TransactionId = "0" };
            Data data =
                new()
                {
                    RutEmisor = rut,
                    DvEmisor = dv,
                    EstadoContab = "REGISTRO",
                    Operacion = "VENTA",
                    Ptributario = Convert.ToDateTime(periodo).ToString("yyyyMM")
                };
            ResResumen? resumen = await service.GetResumen(metaData, data);
            if (resumen != null && resumen.Data != null)
            {
                List<ResDetalle> lista = new();
                List<Task<List<ResDetalle>>> tareas = new();
                tareas = resumen.Data
                    .Select(async b =>
                    {
                        metaData = new() { Namespace = nnssventa, TransactionId = "0" };
                        data = new()
                        {
                            CodTipoDoc = b.RsmnTipoDocInteger.ToString(),
                            RutEmisor = rut,
                            DvEmisor = dv,
                            EstadoContab = "",
                            Operacion = "",
                            Ptributario = Convert.ToDateTime(periodo).ToString("yyyyMM")
                        };
                        ResDetalle? detalle = await service.GetDetalle(
                            metaData,
                            data,
                            MisDteService.UrlDteEmitidos!,
                            canceltoken
                        );
                        lista.Add(detalle!);
                        return lista;
                    })
                    .ToList();
                _ = await Task.WhenAll(tareas);
                return lista;
            }
            return null!;
        }
    }
}
