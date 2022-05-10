//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//using Coordinador.Api;
//using Coordinador.Api.Models;

//using PortalFacturas.Consola.Helpers;
//using PortalFacturas.Consola.Models;
//using PortalFacturas.Consola.Services;

//using Softland.Sql;
//using Softland.Sql.Models;

//namespace PortalFacturas.Consola
//{
//    internal class Start
//    {
//        private static AppSettings _appSettings;
//        private static SshService ftp;
//        private static UnitOfWorkSoftland softland;
//        private static readonly List<Temporal> temporales = new();
//        private static UnitOfWorkCoordinador Cen { get; set; }
//        private static ISharePointService Apisharepoint { get; set; }

//        public Start(
//            ISharePointService _apisharepoint,
//            Microsoft.Extensions.Options.IOptions<AppSettings> appSettings
//        )
//        {
//            Apisharepoint = _apisharepoint;
//            _appSettings = appSettings?.Value;
//            Cen = new UnitOfWorkCoordinador(
//                appSettings.Value.UrlApiCen,
//                _appSettings.UserName,
//                _appSettings.Password
//            );
//        }

//        public async Task DoWorkAsync()
//        {
//            // TESTER


//            // Main
//            Dictionary<string, string> bases = await new UnitOfWorkSoftland().GetDataBases();
//            // Sftp
//            ftp = new(_appSettings.SftpServer);
//            // SharePoint Token
//            await Apisharepoint.RenovarToken();
//            foreach (KeyValuePair<string, string> db in bases)
//            {
//                softland = new(db.Key); // New()!!!
//                // Cen Token
//                await Cen.RenovarToken();
//                int id = await softland.GetExtendedProp();
//                if (id != 0)
//                {
//                    IEnumerable<DteDoccab> docs = await softland.DtedoccabRepo.GetList(
//                        c =>
//                            c.EnviadoSii == 1
//                            && c.FechaGenDte >= new DateTime(2021, 12, 7)
//                            && (c.TipoDte == 33 || c.TipoDte == 61)
//                    //&& c.Marcas == null
//                    //&& c.FechaGenDte > new DateTime(2022, 01, 01)
//                    );
//                    await Temporales(docs);
//                    await UpdateAuxiliares();
//                    await UploadToSharePoint(db.Key, id);
//                    await UploadToSftp(id);
//                    await UploadToCen(id);
//                    ftp.Desconectar();
//                }
//                softland.Dispose();
//            }
//        }

//        private static async Task Temporales(IEnumerable<DteDoccab> docs)
//        {
//            temporales.Clear();
//            List<IwGsaen> tempIwgsaen = new();
//            foreach (DteDoccab item in docs)
//            {
//                if (item.Folio == 51)
//                {
//                    DteDoccab r = docs.FirstOrDefault(c => c.Folio == 51); // ok!!!!!
//                }
//                tempIwgsaen.Add(
//                    await softland.IwgsaenRepo.GetByID(
//                        c => c.Folio == item.Folio && c.FechaGenDte == item.FechaGenDte
//                    )
//                );
//            }
//            // agregar los que falta de cada grupo
//            IEnumerable<IEnumerable<IwGsaen>> grupos = tempIwgsaen
//                .GroupBy(c => c.CodLugarDesp)
//                .Select(g => g.ToList());
//            foreach (List<IwGsaen> item in grupos)
//            {
//                IEnumerable<IwGsaen> listaIwgsaen = await softland.IwgsaenRepo.GetList(
//                    c => c.CodLugarDesp == item.First().CodLugarDesp,
//                    null,
//                    "CodAuxNavigation"
//                );
//                foreach (IwGsaen iw in listaIwgsaen)
//                {
//                    DteDoccab dtedoc = await softland.DtedoccabRepo.GetByFilter(
//                        c => c.FechaGenDte == iw.FechaGenDte && c.Folio == iw.Folio
//                    );
//                    if (dtedoc.EnviadoSii == 1 && (dtedoc.TipoDte == 33 || dtedoc.TipoDte == 61))
//                    {
//                        Console.WriteLine($"{iw.Folio} / {iw.Tipo} / {iw.CodLugarDesp}");
//                        Temporal t =
//                            new()
//                            {
//                                DteDoccab = dtedoc,
//                                IwGsaen = iw,
//                                DteArchivo = await softland.DteArchivoRepo.GetByFilter(
//                                    c =>
//                                        c.NroInt == iw.NroInt
//                                        && c.Folio == iw.Folio
//                                        && c.TipoXml == "SS"
//                                )
//                            };
//                        temporales.Add(t);
//                    }
//                }
//            }
//        }

//        private static async Task UploadToSharePoint(string company, int id)
//        {
//            foreach (Temporal item in temporales)
//            {
//                if (item.DteDoccab.Marcas == null)
//                {
//                    item.DteDoccab.Marcas = await Apisharepoint.UploadStreamAsync(
//                        company,
//                        item.DteArchivo.Archivo,
//                        $"E{id}T{item.DteDoccab.TipoDte}F{item.DteDoccab.Folio}R{item.IwGsaen.CodAuxNavigation.FaxAux1}"
//                    );
//                }
//            }
//        }

//        private static async Task UpdateAuxiliares()
//        {
//            foreach (Temporal item in temporales)
//            {
//                if (
//                    string.IsNullOrEmpty(item.IwGsaen.CodAuxNavigation.FaxAux1)
//                    || string.IsNullOrWhiteSpace(item.IwGsaen.CodAuxNavigation.FaxAux1)
//                )
//                {
//                    item.IwGsaen.CodAuxNavigation.FaxAux1 = (
//                        await Cen.ParticipantRepo.GetByID(item.IwGsaen.CodAux)
//                    ).Id.ToString();
//                    softland.CwtauxiRepo.Update(item.IwGsaen.CodAuxNavigation);
//                }
//            }
//            await softland.Save();
//        }

//        private static async Task UploadToSftp(int id)
//        {
//            // Al FTP val los archivos solamente para cumplir y poder subir un JOB
//            // Podría subir un documento en blanco o con propaganda.
//            IEnumerable<IEnumerable<Temporal>> grupos = temporales
//                .GroupBy(c => new { c.IwGsaen.CodLugarDesp })
//                .Select(g => g.ToList())
//                .ToList();
//            Dictionary<int, string> JsonPassword = await FilesHelper.ReadJsonFile(
//                "sFtp_passwords.json"
//            );
//            foreach (IEnumerable<Temporal> item in grupos)
//            {
//                await FilesHelper.SaveXml(item.ToList(), id);
//                if (
//                    ftp.Conectar(
//                        temporales.First().DteDoccab.Rutemisor.Split('-').First(),
//                        JsonPassword[id]
//                    )
//                )
//                {
//                    ftp.SubirArchivos(item.ToList(), id);
//                }
//            }
//        }

//        private static async Task UploadToCen(int id)
//        {
//            IEnumerable<IEnumerable<Temporal>> grupos = temporales
//                .GroupBy(c => new { c.IwGsaen.CodLugarDesp })
//                .Select(g => g.ToList())
//                .ToList();
//            string rut = temporales.First().DteDoccab.Rutemisor.Split('-').First();
//            foreach (IEnumerable<Temporal> item in grupos)
//            {
//                await FilesHelper.CreateCvs(item.ToList(), id);
//                string naturalKey = $"SEN_{item.First().IwGsaen.DespachadoPor}";
//                string referenceCode = item.First().IwGsaen.CodLugarDesp;
//                int fileId = await Cen.AuxiliaryFileRepo.PutArchivo(naturalKey, rut, id);
//                string idInst = item.First().IwGsaen.SolicitadoPor;
//                if (idInst == null)
//                {
//                    //
//                }
//                InstructionResult inst = await Cen.InstructionRepo.GetByID(idInst);
//                CreditorJobResult job = await Cen.CreditorJobRepo.CrearJob(
//                    fileId,
//                    id,
//                    inst.PaymentMatrix
//                );
//                if (item.First().IwGsaen.CodLugarDesp == "DE05834A21C23S1300")
//                {
//                    //
//                }
//                if (job.Successful)
//                {
//                    CreditorJobResult res = await Publicar(job);
//                    if (res.Successful)
//                    {
//                        if (res.CurrentStep == "publish")
//                        {
//                            Console.WriteLine(
//                                @$"Terminando Job {res.Id} {naturalKey}/{referenceCode}
//                                DELETED:{res.DeleteCount} INSERTED:{res.InsertCount} UPDATED: {res.UpdateCount} TOTAL:{res.ItemsCount} UNCH:{res.UnchangedCount}"
//                            );
//                        }
//                    }
//                    else
//                    {
//                        // No es necesario publicar...
//                    }
//                }
//                else
//                {
//                    // Jon con errores, ej, el xml no está en FTP
//                    /* _loggerHe*/
//                    //  lper.Log($"Error en crear Job {job.Errors[0]}");
//                    return;
//                }
//            }
//        }

//        private static async Task<CreditorJobResult> Publicar(CreditorJobResult job)
//        {
//            CreditorJobResult res = new CreditorJobResult();
//            if (
//                job.UnchangedCount == job.ItemsCount && job.UpdateCount == 0 && job.DeleteCount == 0
//            )
//            {
//                //_loggerHelper.Log($"No es necesario publicar {job.Id}");
//            }
//            else
//            {
//                res = await Cen.CreditorJobRepo.PublicarJob(job);
//                int c = 0;
//                while (res.CurrentStep != "publish" && c < 3)
//                {
//                    Console.WriteLine(
//                        $"Intentando por {c++} vez...{res.CurrentStep} - {res.Expired}"
//                    );
//                    await Task.Delay(2000);
//                    res = await Cen.CreditorJobRepo.PublicarJob(job);
//                }
//            }
//            return res;
//        }
//    }
//}
