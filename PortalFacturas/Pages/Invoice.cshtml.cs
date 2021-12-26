
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Helpers;
using PortalFacturas.Models;
using PortalFacturas.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalFacturas.Pages
{
    public class InvoiceModel : PageModel
    {
        private readonly IApiCenService apiCenService;
        private readonly IXslMapperFunctionService xlstMapperService;
        private readonly IConvertToPdfService convertToPdfService;
        private readonly ISharePointService sharePointService;


        [BindProperty(SupportsGet = true)]
        public int Folio { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        public InvoiceModel(IApiCenService apiCenService, IXslMapperFunctionService xlstMapperService, IConvertToPdfService convertToPdfService, ISharePointService sharePointService)
        {
            this.apiCenService = apiCenService;
            this.xlstMapperService = xlstMapperService;
            this.convertToPdfService = convertToPdfService;
            this.sharePointService = sharePointService;
        }

        //Html
        public async Task<ActionResult> OnGetHtmlDocAsync(int render)
        {
            try
            {
                List<InstructionResult> ejemplo = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
                InstructionResult instruction = ejemplo.FirstOrDefault(c => c.Id == render);
                if (instruction.DteResult.EmissionErpA != null)
                {
                    string EmissionErpA = instruction.DteResult.EmissionErpA;
                    byte[] bytes = await sharePointService
                        .DownloadConvertedFileAsync(EmissionErpA);
                    MemoryStream memoryStream = new(bytes);
                    return new FileStreamResult(memoryStream, "text/html");
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            Mensaje = "No existe ID 'EmissionErpA'";
            return Page();
        }

        //XmlDoc
        public async Task<ActionResult> OnGetXmlDocAsync(int render)
        {
            try
            {
                List<InstructionResult> ejemplo = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
                InstructionResult instruction = ejemplo.FirstOrDefault(c => c.Id == render);
                if (instruction.DteResult.EmissionErpB != null)
                {
                    string EmissionErpB = instruction.DteResult.EmissionErpB;
                    byte[] bytes = await sharePointService
                        .DownloadConvertedFileAsync(EmissionErpB);
                    FileResult fileResult = new FileContentResult(bytes, "application/xml")
                    {
                        FileDownloadName = $"{instruction.DteResult.Folio}.xml"
                    };
                    return fileResult;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            Mensaje = "No existe ID 'EmissionErpB'";
            return Page();
        }


        public async Task<ActionResult> OnGetPdfDocAsync(int render)
        {
            try
            {
                List<InstructionResult> ejemplo = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
                InstructionResult instruction = ejemplo.FirstOrDefault(c => c.Id == render);

                if (instruction.DteResult.ReceptionErp != null)
                {
                    string ReceptionErp = instruction.DteResult.ReceptionErp;
                    byte[] doc = await sharePointService
                        .DownloadConvertedFileAsync(ReceptionErp);
                    byte[] bytes = await convertToPdfService
                        .ConvertToPdf(Encoding.UTF8.GetString(doc));
                    FileResult fileResult = new FileContentResult(bytes, "application/pdf")
                    {
                        FileDownloadName = $"{instruction.DteResult.Folio}.pdf"
                    };
                    return fileResult;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            Mensaje = "No existe ID 'ReceptionErp'";
            return Page();
        }
    }
}
