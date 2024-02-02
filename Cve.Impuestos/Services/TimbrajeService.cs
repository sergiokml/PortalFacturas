using Cve.Impuestos.Helpers;
using Cve.Impuestos.Infraestructure;
using Cve.Impuestos.Services.Interfaces;

namespace Cve.Impuestos.Services
{
    internal class TimbrajeService : ITimbrajeService
    {
        private readonly IRepositoryBaseWeb repo;
        private Dictionary<string, string> InputsText { get; set; }
        public string CantPermitida
        {
            get
            {
                string? c = InputsText.GetValueOrDefault("MAX_AUTOR");
                return c!;
            }
            //set => TxtEmpresaSeleccionada = value;
        }
        public string CantPermitidaSinUsar
        {
            get
            {
                string? c = InputsText.GetValueOrDefault("FOLIOS_DISP");
                return c!;
            }
            //set => TxtEmpresaSeleccionada = value;
        }
        public string MensajeError
        {
            get
            {
                string? c = InputsText.GetValueOrDefault("TABLE"); // InputsText;
                return c!;
            }
            //set => TxtEmpresaSeleccionada = value;
        }

        public TimbrajeService(IRepositoryBaseWeb repo)
        {
            this.repo = repo;
            InputsText = new Dictionary<string, string>();
        }

        /// <summary>
        /// Conecta al sitio con certificado digital y crea las cookies en HttpClient.
        /// </summary>
        /// <returns></returns>
        public async Task<ITimbrajeService> Conectar()
        {
            await repo.GenerarTokenSesion(Properties.Impuestos.UrlSolicitaFolios!);
            return this;
        }

        /// <summary>
        /// Obtención Consumo de Folios
        /// link:Solicitud de timbraje.html
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="dv"></param>
        /// <param name="tipo"></param>
        /// <param name="afectoiva"></param>
        /// <param name="cantidpedir"></param>
        /// <returns></returns>
        public async Task<ITimbrajeService> SolicitaFolios(
            string rut,
            string dv,
            string tipo,
            string afectoiva,
            CancellationToken token
        )
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>("RUT_EMP", rut),
                    new KeyValuePair<string, string>("DV_EMP", dv),
                    new KeyValuePair<string, string>("FOLIO_INICIAL", "0"),
                    new KeyValuePair<string, string>("COD_DOCTO", tipo),
                    new KeyValuePair<string, string>("AFECTO_IVA", afectoiva),
                    new KeyValuePair<string, string>("CON_CREDITO", ""),
                    new KeyValuePair<string, string>("CON_AJUSTE", ""),
                    new KeyValuePair<string, string>("FACTOR", ""),
                    new KeyValuePair<string, string>("CANT_DOCTOS", "")
                };
            HttpResponseMessage? msg = await repo.PostFormWeb(
                values,
                token,
                Properties.Impuestos.UrlSolicitaFolios
            )!;
            InputsText = await HtmlParse.GetValuesFromTag(
                "input[type='text'],input[type='hidden']",
                msg,
                token
            );
            //string? folioMax = InputsText.GetValueOrDefault("MAX_AUTOR");
            return this;
        }

        /// <summary>
        /// Consumo de folios por tipo
        /// link:Confirmacion de timbraje.html
        /// </summary>
        /// <returns></returns>
        public async Task<ITimbrajeService> SeleccionaFolios(
            int cantsolicitada,
            string tipo,
            CancellationToken token
        )
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>(
                        "RUT_EMP",
                        InputsText.GetValueOrDefault("RUT_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "DV_EMP",
                        InputsText.GetValueOrDefault("DV_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_INICIAL",
                        InputsText.GetValueOrDefault("FOLIO_INICIAL")!
                    ),
                    new KeyValuePair<string, string>("COD_DOCTO", tipo), // el 33 está en un cbobox no input
                    new KeyValuePair<string, string>(
                        "AFECTO_IVA",
                        InputsText.GetValueOrDefault("AFECTO_IVA")!
                    ),
                    new KeyValuePair<string, string>(
                        "CON_CREDITO",
                        InputsText.GetValueOrDefault("CON_CREDITO")!
                    ),
                    new KeyValuePair<string, string>(
                        "CON_AJUSTE",
                        InputsText.GetValueOrDefault("CON_AJUSTE")!
                    ),
                    new KeyValuePair<string, string>(
                        "FACTOR",
                        InputsText.GetValueOrDefault("FACTOR")!
                    ),
                    new KeyValuePair<string, string>(
                        "MAX_AUTOR",
                        InputsText.GetValueOrDefault("MAX_AUTOR")!
                    ),
                    new KeyValuePair<string, string>(
                        "ULT_TIMBRAJE",
                        InputsText.GetValueOrDefault("ULT_TIMBRAJE")!
                    ),
                    new KeyValuePair<string, string>(
                        "CON_HISTORIA",
                        InputsText.GetValueOrDefault("CON_HISTORIA")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_INICRE",
                        InputsText.GetValueOrDefault("FOLIO_INICRE")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_FINCRE",
                        InputsText.GetValueOrDefault("FOLIO_FINCRE")!
                    ),
                    new KeyValuePair<string, string>(
                        "FECHA_ANT",
                        InputsText.GetValueOrDefault("FECHA_ANT")!
                    ),
                    new KeyValuePair<string, string>(
                        "ESTADO_TIMBRAJE",
                        InputsText.GetValueOrDefault("ESTADO_TIMBRAJE")!
                    ),
                    new KeyValuePair<string, string>(
                        "CONTROL",
                        InputsText.GetValueOrDefault("CONTROL")!
                    ),
                    new KeyValuePair<string, string>(
                        "CANT_TIMBRAJES",
                        InputsText.GetValueOrDefault("CANT_TIMBRAJES")!
                    ),
                    new KeyValuePair<string, string>("CANT_DOCTOS", cantsolicitada.ToString()),
                    new KeyValuePair<string, string>("ACEPTAR", "Solicitar Numeración"),
                    new KeyValuePair<string, string>(
                        "FOLIOS_DISP",
                        InputsText.GetValueOrDefault("FOLIOS_DISP")!
                    )
                };
            HttpResponseMessage? msg = await repo.PostFormWeb(
                values,
                token,
                Properties.Impuestos.UrlConfirmaFolio
            )!;
            InputsText = await HtmlParse.GetValuesFromTag(
                "input[type='text'],input[type='hidden']",
                msg,
                token
            );

            return this;
        }

        /// <summary>
        /// Genera solicitud Consumo de folios
        /// link:Descargar timbraje.html
        /// </summary>
        /// <returns></returns>
        public async Task<ITimbrajeService> GeneraFolios(CancellationToken token)
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>(
                        "NOMUSU",
                        InputsText.GetValueOrDefault("NOMUSU")!
                    ),
                    new KeyValuePair<string, string>(
                        "CON_CREDITO",
                        InputsText.GetValueOrDefault("CON_CREDITO")!
                    ),
                    new KeyValuePair<string, string>(
                        "CON_AJUSTE",
                        InputsText.GetValueOrDefault("CON_AJUSTE")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIOS_DISP",
                        InputsText.GetValueOrDefault("FOLIOS_DISP")!
                    ),
                    new KeyValuePair<string, string>(
                        "MAX_AUTOR",
                        InputsText.GetValueOrDefault("MAX_AUTOR")!
                    ),
                    new KeyValuePair<string, string>(
                        "ULT_TIMBRAJE",
                        InputsText.GetValueOrDefault("ULT_TIMBRAJE")!
                    ),
                    new KeyValuePair<string, string>(
                        "CON_HISTORIA",
                        InputsText.GetValueOrDefault("CON_HISTORIA")!
                    ),
                    new KeyValuePair<string, string>(
                        "CANT_TIMBRAJES",
                        InputsText.GetValueOrDefault("CANT_TIMBRAJES")!
                    ),
                    new KeyValuePair<string, string>(
                        "CON_AJUSTE",
                        InputsText.GetValueOrDefault("CON_AJUSTE")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_INICRE",
                        InputsText.GetValueOrDefault("FOLIO_INICRE")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_FINCRE",
                        InputsText.GetValueOrDefault("FOLIO_FINCRE")!
                    ),
                    new KeyValuePair<string, string>(
                        "FECHA_ANT",
                        InputsText.GetValueOrDefault("FECHA_ANT")!
                    ),
                    new KeyValuePair<string, string>(
                        "ESTADO_TIMBRAJE",
                        InputsText.GetValueOrDefault("ESTADO_TIMBRAJE")!
                    ),
                    new KeyValuePair<string, string>(
                        "CONTROL",
                        InputsText.GetValueOrDefault("CONTROL")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_INI",
                        InputsText.GetValueOrDefault("FOLIO_INI")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_FIN",
                        InputsText.GetValueOrDefault("FOLIO_FIN")!
                    ),
                    new KeyValuePair<string, string>("DIA", InputsText.GetValueOrDefault("DIA")!),
                    new KeyValuePair<string, string>("MES", InputsText.GetValueOrDefault("MES")!),
                    new KeyValuePair<string, string>("ANO", InputsText.GetValueOrDefault("ANO")!),
                    new KeyValuePair<string, string>("HORA", InputsText.GetValueOrDefault("HORA")!),
                    new KeyValuePair<string, string>(
                        "MINUTO",
                        InputsText.GetValueOrDefault("MINUTO")!
                    ),
                    new KeyValuePair<string, string>(
                        "RUT_EMP",
                        InputsText.GetValueOrDefault("RUT_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "DV_EMP",
                        InputsText.GetValueOrDefault("DV_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "COD_DOCTO",
                        InputsText.GetValueOrDefault("COD_DOCTO")!
                    ),
                    new KeyValuePair<string, string>(
                        "CANT_DOCTOS",
                        InputsText.GetValueOrDefault("CANT_DOCTOS")!
                    ),
                    new KeyValuePair<string, string>("ACEPTAR", "Obtener Folios")
                };
            HttpResponseMessage? msg = await repo.PostFormWeb(
                values,
                token,
                Properties.Impuestos.UrlGeneraFolio
            )!;
            InputsText = await HtmlParse.GetValuesFromTag(
                "input[type='text'],input[type='hidden']",
                msg,
                token
            );
            return this;
        }

        /// <summary>
        /// Descarga el archhivo de Consumo de folios
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<string> DescargaFolio(CancellationToken token)
        {
            List<KeyValuePair<string, string>> values =
                new()
                {
                    new KeyValuePair<string, string>(
                        "RUT_EMP",
                        InputsText.GetValueOrDefault("RUT_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "DV_EMP",
                        InputsText.GetValueOrDefault("DV_EMP")!
                    ),
                    new KeyValuePair<string, string>(
                        "COD_DOCTO",
                        InputsText.GetValueOrDefault("COD_DOCTO")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_INI",
                        InputsText.GetValueOrDefault("FOLIO_INI")!
                    ),
                    new KeyValuePair<string, string>(
                        "FOLIO_FIN",
                        InputsText.GetValueOrDefault("FOLIO_FIN")!
                    ),
                    //new KeyValuePair<string, string>(
                    //    "FECHA",
                    //    $"{InputsText.GetValueOrDefault("ANO")!}-{InputsText.GetValueOrDefault("MES")!}-{InputsText.GetValueOrDefault("DIA")!}"
                    //),
                    new KeyValuePair<string, string>(
                        "FECHA",
                        $"{InputsText.GetValueOrDefault("FECHA")!}"
                    ),
                    new KeyValuePair<string, string>("ACEPTAR", "AQUI")
                };
            string? filename;
            HttpResponseMessage? msg;
            try
            {
                msg = await repo.PostFormWeb(values, token, Properties.Impuestos.UrlGeneraArchivo)!;
                filename = msg.Content.Headers!.ContentDisposition!.FileName;
            }
            catch (Exception)
            {
                return null!;
            }
            using (Stream streamToReadFrom = await msg.Content.ReadAsStreamAsync())
            {
                using Stream streamToWriteTo = File.Open(
                    $"{Path.GetTempPath()}\\{filename}",
                    FileMode.Create
                );
                await streamToReadFrom.CopyToAsync(streamToWriteTo);
            }
            return filename!;
        }
    }
}
