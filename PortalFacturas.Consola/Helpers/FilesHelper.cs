using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using PortalFacturas.Consola.Models;

namespace PortalFacturas.Consola.Helpers
{
    internal static class FilesHelper
    {
        public static async Task SaveXml(List<Temporal> temporales, int id)
        {
            FileInfo p =
                new(
                    $@"{Directory.GetCurrentDirectory()}\{id}\XMLFILES\{temporales.First().IwGsaen.CodLugarDesp}\"
                );
            p.Directory.Create();
            foreach (Temporal item in temporales)
            {
                string filename =
                    $"{temporales.First().DteDoccab.Rutemisor.Split('-').First()}_{item.DteDoccab.TipoDte}_{item.DteDoccab.Folio}";
                if (!File.Exists($@"{p}{filename}.xml"))
                {
                    await File.WriteAllBytesAsync(
                        $@"{p}{filename}.xml",
                        Encoding.UTF8.GetBytes(item.DteArchivo.Archivo)
                    );
                }
            }
        }

        public static async Task CreateCvs(List<Temporal> temporales, int id)
        {
            StringBuilder csv = new();
            csv.AppendLine(
                "id instrucción,Código de referencia,Rol,RUT acreedor,RUT deudor,Monto neto total,Folio,Código DTE SII,Monto bruto,Monto neto,Fecha de emisión,ERP1 emisión,ERP2 emisión,Fecha de entrega de folio,ERP recepción"
            );
            foreach (Temporal item in temporales)
            {
                string newLine = string.Format(
                    "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10:yyyy-MM-dd},{11},{12},{13:yyyy-MM-dd},{14}",
                    item.IwGsaen.SolicitadoPor,
                    temporales.First().IwGsaen.CodLugarDesp,
                    "Acreedor",
                    null,
                    null,
                    null,
                    item.IwGsaen.Folio,
                    item.DteDoccab.TipoDte,
                    item.DteDoccab.MntTotal,
                    item.DteDoccab.MntNeto,
                    item.DteDoccab.FchEmis,
                    item.DteDoccab.Marcas,
                    null,
                    item.DteDoccab.FechaEnvioSii,
                    null
                );
                csv.AppendLine(newLine);
            }
            string path = @$"{Directory.GetCurrentDirectory()}\{id}\CSV\";
            Directory.CreateDirectory(path);
            await File.WriteAllTextAsync(
                $"{path}{$"dtes_acreedor_{temporales.First().DteDoccab.Rutemisor.Split('-').First()}_SEN_{temporales.First().IwGsaen.DespachadoPor}.csv"}",
                csv.ToString(),
                Encoding.UTF8
            );
        }

        public static async Task<Dictionary<int, string>> ReadJsonFile(string jsonName)
        {
            string t = AppContext.BaseDirectory;
            string x = Directory.GetCurrentDirectory();
            using FileStream openStream = File.OpenRead(
                @$"{Directory.GetCurrentDirectory()}/{jsonName}"
            );
            return await JsonSerializer.DeserializeAsync<Dictionary<int, string>>(openStream);
        }

        public static async Task WriteJsonFile(string jsonName, Dictionary<int, string> lista)
        {
            using FileStream createStream = File.Create(
                @$"{Directory.GetCurrentDirectory()}/{jsonName}"
            );
            await JsonSerializer.SerializeAsync(createStream, lista);
        }
    }
}
