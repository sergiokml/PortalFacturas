using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Helpers;
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
        private readonly IApiCenService apiCenService;
        private readonly IXslMapperFunctionService xlstMapperService;
        private readonly ISharePointService sharePointService;


        [BindProperty(SupportsGet = true)]
        public int Folio { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        public InvoiceModel(IApiCenService apiCenService, IXslMapperFunctionService xlstMapperService, ISharePointService sharePointService)
        {
            try
            {
                this.apiCenService = apiCenService;
                this.xlstMapperService = xlstMapperService;
                this.sharePointService = sharePointService;
            }
            catch (Exception)
            {

                throw new Exception("buuuuu");
            }

        }
        public void OnGet()
        {
            //await OnGetHtmlDocAsync(render);

        }

        private DteResult BuscarInst(int render)
        {
            List<InstructionResult> ejemplo = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
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
                    dte.EmissionErpA = "01LOTDAQYY27JRRCMHLRHYSX2VKEZ4FJTW";
                    byte[] bytes = await sharePointService
                        .DownloadConvertedFileAsync(dte.EmissionErpA);

                    byte[] t = await xlstMapperService
                   .LoadXslAsync()
                   .AddParam(bytes)
                   .TransformAsync(bytes);
                    MemoryStream memoryStream = new(t);
                    return new FileStreamResult(memoryStream, "text/html");
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            Mensaje = "Intente nuevamente.";
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
                    byte[] bytes = await sharePointService
                        .DownloadConvertedFileAsync(dte.EmissionErpB);
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


        public async Task<ActionResult> OnGetPdfDocAsync(int render)
        {
            try
            {
                DteResult dte = BuscarInst(render);
                if (dte != null)
                {
                    byte[] doc = await sharePointService
                        .DownloadConvertedFileAsync(dte.ReceptionErp);
                    FileResult fileResult = new FileContentResult(doc, "application/pdf")
                    {
                        FileDownloadName = $"{dte.Folio}.pdf"
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
    }
}
