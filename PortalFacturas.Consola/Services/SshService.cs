using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using PortalFacturas.Consola.Models;

using Renci.SshNet;

namespace PortalFacturas.Consola.Services
{
    internal class SshService
    {
        private readonly string ftpServer;
        private SftpClient SftpClient { get; set; }

        public SshService(string ftpServer)
        {
            this.ftpServer = ftpServer;
        }

        public bool Conectar(string rut, string p)
        {
            string host = ftpServer;
            string username = $"sen{rut}";

            if (SftpClient == null)
            {
                SftpClient = new SftpClient(host, 22, username, p);
            }
            try
            {
                if (!SftpClient.IsConnected)
                {
                    SftpClient.Connect();
                }
                if (SftpClient.IsConnected)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public void Desconectar()
        {
            if (SftpClient.IsConnected)
            {
                SftpClient.Disconnect();
                SftpClient.Dispose();
                SftpClient = null;
            }
        }

        public void SubirArchivos(List<Temporal> temporales, int id)
        {
            string naturalKey = temporales.First().IwGsaen.CodLugarDesp;
            string rut = temporales.First().DteDoccab.Rutemisor.Split('-').First();
            //string refrenceCode = temporales.First().IwGsaen.DespachadoPor;
            // Lista de subidos para no repetir
            List<string> files = GetFiles(@$"/sen{rut}/DTE/{naturalKey}/");
            foreach (Temporal item in temporales)
            {
                string filename = $"{rut}_{item.DteDoccab.TipoDte}_{item.DteDoccab.Folio}";
                string remote = @$"/sen{rut}/DTE/{naturalKey}/{filename}.xml";
                string local =
                    @$"{Directory.GetCurrentDirectory()}/{id}/XMLFILES/{naturalKey}/{filename}.xml";

                if (files == null || files.Count == 0 || !files.Contains($"{filename}.xml"))
                {
                    using (FileStream fs = new(local, FileMode.Open))
                    {
                        //SftpClient.BufferSize = 1024;
                        SftpClient.UploadFile(fs, remote);
                        Console.WriteLine(
                            $"Subiendo archivo {filename}.xml a FTP {id} {naturalKey}"
                        );
                    }
                }
            }
        }

        private List<string> GetFiles(string path)
        {
            if (SftpClient.Exists(path))
            {
                return SftpClient
                    .ListDirectory(path)
                    .Where(f => !f.IsDirectory)
                    .Select(f => f.Name)
                    .ToList();
            }
            return null;
        }
    }
}
