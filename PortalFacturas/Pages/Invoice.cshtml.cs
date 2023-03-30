using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Cve.Coordinador.Models;
using Cve.Impuestos.Helpers;
using Cve.Notificacion;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Helpers;
using PortalFacturas.Interfaces;

namespace PortalFacturas.Pages
{
    [Authorize]
    public class InvoiceModel : PageModel
    {
        private readonly IConvertToPdfService convertToPdfService;
        private readonly GraphService graph;
        private readonly XsltHelper xsltService = new();

        [BindProperty(SupportsGet = true)]
        public int Folio { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        public InvoiceModel(IConvertToPdfService convertToPdfService, GraphService graph)
        {
            this.convertToPdfService = convertToPdfService;
            this.graph = graph;
        }

        public void OnGet()
        {
            //await OnGetHtmlDocAsync(render);
        }

        private Dte BuscarInst(int render)
        {
            List<Instruction> temp = SessionHelperExtension.GetObjectFromJson<List<Instruction>>(
                HttpContext.Session,
                "Instrucciones"
            );

            try
            {
                return temp.SelectMany(store => store.DteAsociados)
                    .Where(address => address.Id == render)
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception("");
            }
        }

        //Html
        public async Task<ActionResult> OnGetHtmlDocAsync(string render)
        {
            try
            {
                var dte = BuscarInst(Convert.ToInt32(render));
                if (dte.EmissionErpA != "0")
                {
                    var bytes = await graph.BajarFile(dte.EmissionErpA);
                    XDocument respnseXml = XDocument.Parse(Encoding.UTF8.GetString(bytes));
                    var b = respnseXml
                        .Descendants()
                        .First(p => p.Name.LocalName == "DTE")
                        .ToString();
                    var html = xsltService.GenerateHtml(b);
                    //MemoryStream memoryStream = new(html);
                    return new ContentResult { Content = html, ContentType = "text/html" };
                    //return new FileStreamResult(memoryStream, "text/html");
                }
                else
                {
                    Mensaje = $"El documento no ha sido subido al Drive.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"No se puede mostrar el documento: {ex.Message}";
            }
            return Page();
        }

        //XmlDoc
        public async Task<ActionResult> OnGetXmlDocAsync(int render)
        {
            try
            {
                Dte dte = BuscarInst(render);
                if (dte.EmissionErpA != "0")
                {
                    var bytes = await graph.BajarFile(dte.EmissionErpA);
                    return File(bytes, "application/xml", $"{GetFileName(dte)}.xml");
                }
                else
                {
                    Mensaje = $"El documento no ha sido subido al Drive.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"No se puede mostrar el documento: {ex.Message}";
            }
            return Page();
        }

        //PdfDoc
        public async Task<ActionResult> OnGetPdfDocAsync(int render)
        {
            try
            {
                Dte dte = BuscarInst(Convert.ToInt32(render));
                if (dte.EmissionErpA != "0")
                {
                    var bytes = await graph.BajarFile(dte.EmissionErpA);
                    XDocument respnseXml = XDocument.Parse(Encoding.UTF8.GetString(bytes));
                    var b = respnseXml
                        .Descendants()
                        .First(p => p.Name.LocalName == "DTE")
                        .ToString();
                    var html = xsltService.GenerateHtml(b);
                    byte[] pdf = await convertToPdfService.ConvertToPdf(html, GetFileName(dte));
                    return File(pdf, "application/pdf", $"{GetFileName(dte)}.pdf");
                }
                else
                {
                    Mensaje = $"El documento no ha sido subido al Drive.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"No se puede mostrar el documento: {ex.Message}";
            }
            return Page();
        }

        private string GetFileName(Dte dte)
        {
            string filename = string.Empty;
            if (dte.Type == 1) //33
            {
                filename = $"{TempData["EmisorID"]}_33_{dte.Folio}";
            }
            else if (dte.Type == 2) //61
            {
                filename = $"{TempData["EmisorID"]}_61_{dte.Folio}";
            }
            TempData.Keep("EmisorID");
            return filename;
        }
    }
}
