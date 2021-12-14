
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Services;

using System;
using System.IO;
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
        public async Task<ActionResult> OnGetHtmlDocAsync(string renderpath)
        {
            try
            {
                // Test
                renderpath = "6741.xml";

                byte[] x = await sharePointService.DownloadConvertedFileAsync("01TPAHJKQLYYVAQQWNAZCJUDY4H2K2AQ73", "html");

                //string url = DecodificarUrlAsync(renderpath);
                //string res = await apiCenService.ConvertDocument(url);
                //string responseModel = await xlstMapperService.ConvertDocument(res);

                // byte[] bytes = Encoding.UTF8.GetBytes(responseModel);
                MemoryStream memoryStream = new(x);
                return new FileStreamResult(memoryStream, "text/html");
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            return Page();
        }

        //XmlDoc
        public async Task<ActionResult> OnGetXmlDocAsync(string renderpath)
        {
            try
            {
                string url = DecodificarUrlAsync(renderpath);
                byte[] bytes = Encoding.UTF8.GetBytes(await apiCenService.ConvertDocument(url));
                FileResult fileResult = new FileContentResult(bytes, "application/xml")
                {
                    FileDownloadName = $"{Folio}.xml"
                };
                return fileResult;
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            return Page();
        }


        public async Task<ActionResult> OnGetPdfDocAsync(string renderpath)
        {
            try
            {
                string url = DecodificarUrlAsync(renderpath);
                string res = await apiCenService.ConvertDocument(url);
                string responseModel = await xlstMapperService.ConvertDocument(res);

                //Pdf  
                byte[] bytes = await convertToPdfService.ConvertToPdf(responseModel);
                FileResult fileResult = new FileContentResult(bytes, "application/pdf")
                {
                    FileDownloadName = $"{Folio}.pdf"
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
