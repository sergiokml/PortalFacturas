using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Renci.SshNet;

namespace Cve.Notificacion
{
    public class SftpService
    {
        private readonly ILogger logger;
        private readonly IConfiguration config;
        public SftpClient client { get; set; } = null!;

        public SftpService(ILogger<SftpService> logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        public bool ConectarFtp(string rut, string ftptkn)
        {
            string host = config.GetSection("Cen:UrlFtp").Value!;
            string username = $"sen{rut}";
            client = new SftpClient(host, 22, username, ftptkn);
            try
            {
                client.Connect();
                if (client.IsConnected)
                {
                    logger.LogWarning(
                        $"Conectado correctamente, directorio: {client.WorkingDirectory}"
                    );
                    return true;
                }
                else
                {
                    logger.LogWarning($"No se pudo conectar a FTP {host}-{username}");
                }
            }
            catch (Exception e)
            {
                logger.LogError($"ConectarFtp {e.Message}");
                return false;
            }
            return false;
        }

        public bool SubirArchivos(string dbId, string referenceCode, string filename, string rut)
        {
            string remote = @$"/sen{rut}/DTE/{referenceCode}/{filename}.xml";
            string local =
                @$"{Directory.GetCurrentDirectory()}/{dbId}/XMLFILES/{referenceCode}/{filename}.xml";
            try
            {
                using FileStream fs = new(local, FileMode.Open);
                //SftpClient.BufferSize = 1024;
                client.UploadFile(fs, remote);
                Console.WriteLine($"Subiendo archivo {filename}.xml a FTP");
                logger.LogWarning(
                    $"Archivo subido a FTP correctamente {filename} - {dbId} - {rut}"
                );
                return true;
            }
            catch (Exception e)
            {
                logger.LogError($"ConectarFtp {e.Message}");
                return false;
            }
        }

        public List<string> GetFiles(string path)
        {
            try
            {
                if (client.Exists(path))
                {
                    return client
                        .ListDirectory(path)
                        .Where(f => !f.IsDirectory)
                        .Select(f => f.Name)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error en conseguir archivos del directorio. {e.Message}");
            }
            return null!;
        }
    }
}
