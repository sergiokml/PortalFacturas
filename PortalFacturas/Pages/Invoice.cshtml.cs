using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Helpers;
using PortalFacturas.Interfaces;
using PortalFacturas.Models;
using PortalFacturas.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PortalFacturas.Pages
{
    [Authorize]
    public class InvoiceModel : PageModel
    {
        private readonly IXsltHelper xlstMapperService;
        private readonly ISharePointService sharePointService;
        private readonly IConvertToPdfService convertToPdfService;

        [BindProperty(SupportsGet = true)]
        public int Folio { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        public InvoiceModel(
            ISharePointService sharePointService,
            IConvertToPdfService convertToPdfService
        )
        {
            xlstMapperService = new XsltHelper();
            this.sharePointService = sharePointService;
            this.convertToPdfService = convertToPdfService;
        }

        public void OnGet()
        {
            //await OnGetHtmlDocAsync(render);
        }

        private DteResult BuscarInst(int render)
        {
            List<InstructionResult> ejemplo = SessionHelperExtension.GetObjectFromJson<
                List<InstructionResult>
            >(HttpContext.Session, "Instrucciones");
            foreach (InstructionResult item in ejemplo)
            {
                try
                {
                    List<DteResult> dtes = item.DteResult;
                    if (dtes != null)
                    {
                        foreach (DteResult d in dtes)
                        {
                            if (d.Id == render)
                            {
                                return d;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("");
                }
            }
            return null;
        }

        //Html
        public async Task<ActionResult> OnGetHtmlDocAsync(string render)
        {
            try
            {
                DteResult dte = BuscarInst(Convert.ToInt32(render));
                byte[] bytes = await sharePointService.DownloadFileAsync(dte.EmissionErpA);
                XDocument respnseXml = XDocument.Parse(bytes.ToString(false));
                byte[] b = respnseXml
                    .Descendants()
                    .First(p => p.Name.LocalName == "DTE")
                    .ToString()
                    .ToBytes(false);

                byte[] t = await xlstMapperService.LoadXslAsync().AddParam(b).TransformAsync(b);
                MemoryStream memoryStream = new(t);
                return new FileStreamResult(memoryStream, "text/html");
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
                DteResult dte = BuscarInst(render);
                byte[] bytes = await sharePointService.DownloadFileAsync(dte.EmissionErpA);
                return File(bytes, "application/xml", $"{GetFileName(dte)}.xml");
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
                DteResult dte = BuscarInst(Convert.ToInt32(render));
                byte[] bytes = await sharePointService.DownloadFileAsync(dte.EmissionErpA);
                XDocument respnseXml = XDocument.Parse(bytes.ToString(false));
                byte[] b = respnseXml
                    .Descendants()
                    .First(p => p.Name.LocalName == "DTE")
                    .ToString()
                    .ToBytes(false);

                byte[] html = await xlstMapperService.LoadXslAsync().AddParam(b).TransformAsync(b);
                byte[] pdf = await convertToPdfService.ConvertToPdf(
                    html.ToString(false),
                    GetFileName(dte)
                );
                return File(pdf, "application/pdf", $"{GetFileName(dte)}.pdf");
            }
            catch (Exception ex)
            {
                Mensaje = $"No se puede mostrar el documento: {ex.Message}";
            }
            return Page();
        }

        private string GetFileName(DteResult dte)
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
