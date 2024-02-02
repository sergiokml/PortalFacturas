//using Cve.Impuestos.Models;

//using Microsoft.Extensions.Configuration;

//using ServiceEstadoDteAv;

//namespace Cve.Impuestos.Connected_Services
//{
//    internal class WcfConsultaAvanzadaEstadoDte : WcfSemillaToken
//    {
//        public WcfConsultaAvanzadaEstadoDte(IConfiguration config)
//            : base(config) { }

//        // SERVICIO SOAP ServiceEstadoDteAv
//        // https://www.sii.cl/factura_electronica/factura_mercado/OIFE2006_QueryEstDteAv_MDE.pdf
//        // NO MUESTRA "RECLAMADOS" PERO SÍ NC EMITIDAS

//        public async Task<RESPUESTA> ConsultaAvanzadaEstadoDte(
//            string RutEmpresa,
//            string DvEmpresa,
//            string RutReceptor,
//            string DvReceptor,
//            string TipoDte,
//            string FolioDte,
//            string FechaEmisionDte,
//            string MontoDte,
//            string FirmaDte
//        )
//        {
//            //  NO ESTÁ PROBADO MASIVAMENTE ESTA INSTANCIA DEL CLIENTE
//            var cliente = new QueryEstDteAvClient();
//            var resp = await cliente.getEstDteAvAsync(
//                RutEmpresa,
//                DvEmpresa,
//                RutReceptor,
//                DvReceptor,
//                TipoDte,
//                FolioDte,
//                FechaEmisionDte,
//                MontoDte,
//                FirmaDte,
//                Token
//            );
//            return Deserialize<RESPUESTA>(resp);
//        }

//        public override async Task EnsureSuccessStatus(HttpResponseMessage response)
//        {
//            if (!response.IsSuccessStatusCode)
//            {
//                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//                {
//                    await GetToken();
//                }
//                else
//                {
//                    throw new Exception(
//                        $"{response.StatusCode}:{response.RequestMessage!.RequestUri}"
//                    );
//                }
//            }
//        }
//    }
//}
