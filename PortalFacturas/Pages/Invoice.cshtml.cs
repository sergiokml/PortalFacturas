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

            for (int i = 0; i < ejemplo.Count; i++)
            {
                InstructionResult nrodte = ejemplo.ElementAt(i);
                if (nrodte.DteResult != null)
                {
                    for (int z = 0; z < nrodte.DteResult.Count; z++)
                    {
                        DteResult dte = nrodte.DteResult[z];
                        if (dte.Id == render)
                        {
                            return dte;
                        }
                    }
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
                if (dte != null)
                {
                    // Debo serializar de acuerdo al DTE => 2 tipos
                    byte[] bytes = await sharePointService.DownloadConvertedFileAsync(
                        dte.EmissionErpA
                    );
                    byte[] t = await xlstMapperService
                        .LoadXslAsync()
                        .AddParam(bytes)
                        .TransformAsync(bytes);

                    string test = t.ToString(false);

                    MemoryStream memoryStream = new(t);
                    return new FileStreamResult(memoryStream, "text/html");
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
                DteResult dte = BuscarInst(render);
                if (dte != null)
                {
                    byte[] bytes = await sharePointService.DownloadConvertedFileAsync(
                        dte.EmissionErpA
                    );
                    FileResult fileResult = new FileContentResult(bytes, "application/xml")
                    {
                        FileDownloadName = $"{dte.Folio}.xml"
                    };
                    return fileResult;
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
                DteResult dte = BuscarInst(render);
                if (dte != null)
                {
                    // Debo serializar de acuerdo al DTE => 2 tipos
                    byte[] bytes = await sharePointService.DownloadConvertedFileAsync(
                        dte.EmissionErpA
                    );
                    byte[] t = await xlstMapperService
                        .LoadXslAsync()
                        .AddParam(bytes)
                        .TransformAsync(bytes);

                    string test = t.ToString(false);

                    string pdf = await convertToPdfService.ConvertToPdf(t.ToString(false));

                    return Redirect(pdf);
                    //FileResult fileResult = new FileContentResult(pdf, "application/pdf")
                    //{
                    //    FileDownloadName = $"{dte.Folio}.pdf"
                    //};
                    return null;
                }
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
