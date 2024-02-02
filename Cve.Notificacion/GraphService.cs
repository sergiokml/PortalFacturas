using System.IO.Compression;
using System.Text;

using Azure.Core;
using Azure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Kiota.Abstractions.Authentication;

using MimeKit;

namespace Cve.Notificacion
{
    public class GraphService
    {
        private readonly ILogger logger;
        private readonly IConfiguration config;
        private const string Folder = @"TEMP_INBOX_DTE\";

        //  private const string DriveId =
        //    "b!QNy1xUqqc0Owm22y-Ov2gmJE_m-3dp9KvOS4L_uLuCsQlb9IdSw4QZV4MlySnrQE";

        public GraphService(ILogger<GraphService> logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        private GraphServiceClient Auth()
        {
            // AUTH PARA PERMISOS DELEGADOS
            // REVISAR CORREOS
            // ENVIAR EMAIL
            try
            {
                BaseBearerTokenAuthenticationProvider accessTokenProvider =
                    new(new TokenProvider(config, logger));
                GraphServiceClient graphServiceClient = new(accessTokenProvider);
                Console.WriteLine("Retornando Auth Delegado..............................");
                return graphServiceClient;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        public TokenCredential? Auth2()
        {
            // AUTH PARA CREAR DIRECTORIO EN EL SITIO SP Y SUBIR FILES
            // PERMISOS DE APLICACIÓN NO LLEVAN USER + PASS
            ClientSecretCredential clientSecretCredential =
                new(
                    config.GetSection("ADConfig:TenantId").Value!,
                    config.GetSection("ADConfig:ClientId").Value!,
                    config.GetSection("ADConfig:ClientSecret").Value!,
                    new() { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud }
                );
            Console.WriteLine($"Retornando Auth App..............................");
            return clientSecretCredential;
        }

        public async Task<List<Attachment>> LeerCarpetas(string fecha)
        {
            string folder = @$"{Path.GetTempPath()}{Folder}";
            if (Directory.Exists(folder))
            {
                DirectoryInfo di = new(folder);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            else
            {
                _ = Directory.CreateDirectory(folder);
            }
            List<Attachment> listaMsjes = await GetMensajes(fecha, "Inbox");
            listaMsjes.AddRange(await GetMensajes(fecha, "JunkEmail"));
            logger.LogWarning($"Leyendo {listaMsjes.Count} mensajes en total.");
            GraphServiceClient graphClient = Auth();
            foreach (Attachment item in listaMsjes)
            {
                if (item.ContentType is "message/rfc822" or "text/rfc822-headers")
                {
                    try
                    {
                        Stream? g = await graphClient.Me.Messages[item.Id].Content.GetAsync();
                        if (g != null)
                        {
                            await GuardarAdjunto(g);
                        }
                    }
                    catch (ODataError e)
                    {
                        Console.WriteLine(e.Error!.Code);
                        Console.WriteLine(e.Error.Message);
                        logger.LogError(e.Message);
                    }
                }
                else
                {
                    await GuardarAdjunto(item!);
                }
            }
            return listaMsjes;
        }

        private async Task<List<Attachment>> GetMensajes(string today, string folder)
        {
            int count = 0;
            today ??= DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            logger.LogWarning("Leyendo Carpeta {a} con Fecha desde {b}.", folder, today);
            GraphServiceClient graphClient = Auth();
            MessageCollectionResponse? messages = await graphClient.Me.MailFolders[
                folder
            ].Messages.GetAsync(
                a =>
                {
                    a.QueryParameters.Top = 100;
                    a.QueryParameters.Select = new string[]
                    {
                        "id",
                        "subject",
                        "from",
                        "receiveddatetime"
                    };
                    a.QueryParameters.Filter = $"receivedDateTime ge {today}";
                },
                new CancellationToken()
            );
            List<Attachment> list = new();
            PageIterator<Message, MessageCollectionResponse> iterator = PageIterator<
                Message,
                MessageCollectionResponse
            >.CreatePageIterator(
                graphClient,
                messages!,
                async (m) =>
                {
                    ++count;
                    if (
                        m.From!.EmailAddress!.Address != config.GetSection("EmailConfig:User").Value
                        && m.From.EmailAddress.Address != "siidte_error@sii.cl"
                        && m.From.EmailAddress.Address != "siidte@sii.cl"
                    )
                    {
                        try
                        {
                            Console.WriteLine(
                                $"{count} Leyendo: {m.ReceivedDateTime!.Value:dd-MM-yyyy HH:mm:ss}- {m.From.EmailAddress.Address}"
                            );
                            List<Attachment>? att = (
                                await graphClient.Me.Messages[m.Id].Attachments.GetAsync()
                            )!.Value;
                            list.AddRange(att!);
                            // UPDATE
                            //m.IsRead = true;
                            //_ = await graphClient.Me.Messages[m.Id].PatchAsync(m);
                        }
                        catch (ODataError e)
                        {
                            Console.WriteLine(e.Error!.Code);
                            Console.WriteLine(e.Error.Message);
                            logger.LogError(e.Message);
                        }
                        catch (Exception e)
                        {
                            logger.LogError($"{m.ReceivedDateTime}-{m.Id}-{m.Subject}:{e.Message}");
                        }
                    }
                    return true;
                }
            );
            await iterator.IterateAsync();
            logger.LogWarning("Encontrados {a} correos en carpeta {b}.", list.Count, folder);
            return list!;
        }

        private async Task GuardarAdjunto(Stream msje)
        {
            List<MimePart> attachments = new();
            MimeMessage message = MimeMessage.Load(msje);
            MimeIterator iter = new(message);
            while (iter.MoveNext())
            {
                if (iter.Parent is Multipart && iter.Current is MimePart part && part.IsAttachment)
                {
                    attachments.Add(part);
                }
            }
            foreach (MimePart item in attachments)
            {
                try
                {
                    using FileStream stream = File.Create(
                        $"{Path.GetTempPath()}{Folder}{item.FileName}"
                    );
                    await item.Content.DecodeToAsync(stream);
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    continue;
                }
            }
        }

        private async Task GuardarAdjunto(Attachment att)
        {
            try
            {
                string? newname = string.Empty;
                if (att.Name == null || string.IsNullOrEmpty(att.Name))
                {
                    newname = $"{Path.GetTempPath()}{Folder}{(int)DateTime.Now.Ticks}_.xml";
                }
                else
                {
                    newname = $"{Path.GetTempPath()}{Folder}{Path.GetFileName(att.Name)}";
                    if (File.Exists(newname))
                    {
                        newname =
                            $"{Path.GetTempPath()}{Folder}{(int)DateTime.Now.Ticks}_{Path.GetFileName(att.Name)}";
                    }
                }
                if (Path.GetExtension(newname) is ".xml" or ".zip")
                {
                    byte[] file = ((FileAttachment)att)!.ContentBytes!;
                    if (file.Length > 0)
                    {
                        await File.WriteAllBytesAsync(newname!, file);
                    }
                }
                if (Path.GetExtension(newname) == ".zip")
                {
                    ZipFile.ExtractToDirectory(newname!, $"{Path.GetTempPath()}{Folder}", true);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        public async Task EnviarMensaje(
            string[] pathAdjunto,
            string asunto,
            string body,
            bool cc = true
        )
        {
            GraphServiceClient graphClient = Auth();
            Message message =
                new()
                {
                    Subject = asunto,
                    Body = new ItemBody { ContentType = BodyType.Html, Content = body }
                };
            List<Recipient> tos = new();
            Recipient dev =
                new()
                {
                    EmailAddress = new EmailAddress() { Address = "sergio.programador@outlook.com" }
                };
            tos.Add(dev);
            if (cc)
            {
                Recipient to =
                    new()
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = config.GetSection("EmailConfig:UserCC").Value!
                        }
                    };
                tos.Add(to);
            }
            message.ToRecipients = tos;
            List<Attachment> page = new();
            foreach (string item in pathAdjunto)
            {
                using FileStream stream = System.IO.File.Open(
                    item,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );
                using MemoryStream memoryStream = new();
                stream.CopyTo(memoryStream);
                FileAttachment att =
                    new()
                    {
                        ContentBytes = memoryStream.ToArray(),
                        ContentType = "text/plain",
                        Name = Path.GetFileName(item)
                    };
                page.Add(att);
            }
            message.Attachments = page;
            await graphClient.Users[
                config.GetSection("EmailConfig:User").Value!
            ].SendMail.PostAsync(
                new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody()
                {
                    Message = message
                }
            );
        }

        public async Task EnviarErrores(string name)
        {
            // ENVIAR EMAIL CON ERRORES
            string path =
                $@"{Environment.CurrentDirectory}\logs\log_app_Error_{DateTime.Now:yyyy}-{DateTime.Now:MM}-{DateTime.Now:dd}.txt";

            FileInfo info = new(path);
            if (info.Exists && info.Length != 0)
            {
                await EnviarMensaje(
                    new string[] { path },
                    $"[Centralizador+] Errores SCRIPT: {name}.",
                    null!,
                    false
                );
            }
        }

        public async Task<string> SubirFile(DriveItem item, string contents, string name)
        {
            GraphServiceClient graphClient = new(Auth2());
            MemoryStream stream = new(Encoding.UTF8.GetBytes(contents));
            try
            {
                //
                await graphClient.Drives[config.GetSection("ADConfig:DriveId").Value!].Items[
                    item.Id
                ]
                    .ItemWithPath(name)
                    .Content.PutAsync(stream, null, new CancellationToken());

                DriveItem? newDriveItem = await graphClient.Drives[
                    config.GetSection("ADConfig:DriveId").Value!
                ].Items[item.Id]
                    .ItemWithPath(name)
                    .GetAsync();

                //DriveItemCollectionResponse? buscar = await graphClient.Drives[DriveId].Items[
                //    item.Id
                //].Children.GetAsync(a =>
                //{
                //    a.QueryParameters.Select = new string[] { "id" };
                //    a.QueryParameters.Filter = $"name eq '{name}'";
                //});
                //DriveItem? li = buscar!.Value!.FirstOrDefault();
                Console.WriteLine($"Subiendo File a {item.Name}-{name}-{newDriveItem!.Id}");
                return newDriveItem!.Id!;
            }
            catch (ODataError odataError)
            {
                Console.WriteLine(odataError.Error!.Code);
                Console.WriteLine(odataError.Error.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<DriveItem?> CrearCarpeta(string carpeta)
        {
            GraphServiceClient graphClient = new(Auth2());
            try
            {
                return await graphClient.Drives[config.GetSection("ADConfig:DriveId").Value!].Root
                    .ItemWithPath(carpeta)
                    .GetAsync();
            }
            catch (ODataError odataError)
            {
                if (odataError.Error!.Code == "itemNotFound")
                {
                    // CREATE
                    return await graphClient.Drives[
                        config.GetSection("ADConfig:DriveId").Value!
                    ].Items.PostAsync(new DriveItem() { Name = carpeta, Folder = new Folder() });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null!;
        }

        public async Task<byte[]> BajarFile(string id)
        {
            GraphServiceClient graphClient = new(Auth2());
            var stream = await graphClient.Drives[
                config.GetSection("ADConfig:DriveId").Value!
            ].Items[id].Content.GetAsync(null, new CancellationToken());
            using MemoryStream ms = new();
            stream!.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
