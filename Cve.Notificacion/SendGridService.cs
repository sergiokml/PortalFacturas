using System.Text;

using Microsoft.Extensions.Configuration;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace Cve.Notificacion
{
    public class SendGridService
    {
        private readonly IConfiguration config = null!;

        public SendGridService(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<Response> Enviar(
            string xml,
            string asunto,
            string htmlcontent,
            string to,
            string namefile
        )
        {
            SendGridClient? client = new(config.GetSection("EmailConfig:SendGridKey").Value!);
            SendGridMessage? msg =
                new()
                {
                    From = new EmailAddress(
                        config.GetSection("EmailConfig:UserSendGrid").Value!,
                        "Facturation Chili"
                    ),
                    Subject = asunto,
                    HtmlContent = htmlcontent
                };
            msg.AddTo(new EmailAddress(to));
            msg.AddTo(new EmailAddress(config.GetSection("EmailConfig:UserCC").Value!));
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(xml);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            //msg.AddTo(new EmailAddress("sergio.programador@outlook.com"));
            MemoryStream? streamm = new(isoBytes);
            await msg.AddAttachmentAsync(namefile, streamm, "text/xml", "attachment");
            return await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}
