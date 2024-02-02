//using System.Net.Http.Json;

//using Cve.Impuestos.Models;

//using Microsoft.Extensions.Configuration;

//namespace Cve.Impuestos.Connected_Services
//{
//    internal class WcfConsultaRegistroDte : WcfSemillaToken
//    {
//        public WcfConsultaRegistroDte(IConfiguration config)
//            : base(config) { }

//        public Task<RESPUESTA> ConsultaListadoEventos(
//            string rutEmisor,
//            string dvEmisor,
//            string tipoDoc,
//            string folio
//        )
//        {
//            // **************** ESTE MÉTODO NO FUNCIONA !!!!!!!!!!!!!!!!!!!!!!!!!!! (NATIVO WCF)
//            var cliente = new ServiceReclamaAceptaConsultaDte.RegistroReclamoDteServiceClient();
//            //using OperationContextScope ocs = new(client.InnerChannel);
//            //var requestProp = new HttpRequestMessageProperty();
//            //requestProp.Headers["Cookie"] = $"Token={Token}";
//            //OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
//            //    requestProp;
//            var resp = cliente.listarEventosHistDoc(rutEmisor, dvEmisor, tipoDoc, folio);
//            //return Deserializa(resp);
//            return null!;
//        }

//        public async override Task EnsureSuccessStatus(HttpResponseMessage response)
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

//        public async Task<RESPUESTA?> listarEventosHistDoc(
//            string rutEmisor,
//            string dvEmisor,
//            string tipoDoc,
//            string folio
//        )
//        {
//            HttpClient client = new();
//            HttpRequestMessage request =
//                new(
//                    HttpMethod.Post,
//                    "https://ws1.sii.cl/WSREGISTRORECLAMODTE/registroreclamodteservice"
//                );
//            request.Headers.Add("Cookie", $"TOKEN={Token}");
//            StringContent content =
//                new(
//                    "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" "
//                        + "xmlns:ws=\"http://ws.registroreclamodte.diii.sdi.sii.cl\">\r\n"
//                        + "<soapenv:Header/>\r\n"
//                        + "<soapenv:Body>\r\n"
//                        + "<ws:listarEventosHistDoc>\r\n"
//                        + $"<rutEmisor>{rutEmisor}</rutEmisor>\r\n"
//                        + $"<dvEmisor>{dvEmisor}</dvEmisor>\r\n"
//                        + $"<tipoDoc>{tipoDoc}</tipoDoc>\r\n"
//                        + $"<folio>{folio}</folio>\r\n"
//                        + "</ws:listarEventosHistDoc>\r\n"
//                        + "</soapenv:Body>\r\n</soapenv:Envelope>",
//                    null,
//                    "application/xml"
//                );
//            request.Content = content;
//            HttpResponseMessage msg = await client.SendAsync(request);
//            await EnsureSuccessStatus(msg);
//            return await msg.Content!.ReadFromJsonAsync<RESPUESTA>()!;
//        }
//    }
//}
