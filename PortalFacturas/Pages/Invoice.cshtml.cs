
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
                // Test
                //var ERP1Emisi�n = instruction.DteResult.EmissionErpA;
                string ERP1Emisi�n = "01TPAHJKURTVNUEHYI5BD2MFDEUX4ZED2F";
                byte[] bytes = await sharePointService
                    .DownloadConvertedFileAsync(ERP1Emisi�n);
                MemoryStream memoryStream = new(bytes);
                return new FileStreamResult(memoryStream, "text/html");
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            return Page();
        }

        //XmlDoc
        public async Task<ActionResult> OnGetXmlDocAsync(int render)
        {
            try
            {
                List<InstructionResult> ejemplo = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
                InstructionResult instruction = ejemplo.FirstOrDefault(c => c.Id == render);

                // Tester
                //var ERP1Emisi�n = instruction.DteResult.EmissionErpA;
                string ERP1Emisi�n = "01TPAHJKTBPGWPNNUUDFGYFYEAX5Q34E37";
                byte[] bytes = await sharePointService
                    .DownloadConvertedFileAsync(ERP1Emisi�n);
                FileResult fileResult = new FileContentResult(bytes, "application/xml")
                {
                    FileDownloadName = $"{instruction.DteResult.Folio}.xml"
                };
                return fileResult;
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            return Page();
        }


        public async Task<ActionResult> OnGetPdfDocAsync(int render)
        {
            try
            {
                List<InstructionResult> ejemplo = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
                InstructionResult instruction = ejemplo.FirstOrDefault(c => c.Id == render);

                // Test
                //var ERP1Emisi�n = instruction.DteResult.EmissionErpA;
                string ERP1Emisi�n = "01TPAHJKURTVNUEHYI5BD2MFDEUX4ZED2F";
                //Html
                byte[] doc = await sharePointService
                    .DownloadConvertedFileAsync(ERP1Emisi�n);

                //Pdf  
                byte[] bytes = await convertToPdfService
                    .ConvertToPdf(Encoding.UTF8.GetString(doc));

                FileResult fileResult = new FileContentResult(bytes, "application/pdf")
                {
                    FileDownloadName = $"{instruction.DteResult.Folio}.pdf"
                };
                return fileResult;
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            return Page();
        }

        private static string DecodificarUrlAsync(string renderpath)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(renderpath);
            return Encoding.ASCII.GetString(encodedDataAsBytes);
        }

    }

}
