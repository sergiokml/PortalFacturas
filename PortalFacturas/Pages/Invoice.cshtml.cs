
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Interfaces;
using PortalFacturas.Models;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PortalFacturas.Pages
{
    public class InvoiceModel : PageModel
    {
        private readonly IApiCenService apiCenService;

        [BindProperty(SupportsGet = true)]
        public int Folio { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        public InvoiceModel(IApiCenService apiCenService)
        {
            this.apiCenService = apiCenService;
        }

        //Html
        public async Task<ActionResult> OnGetHtmlDocAsync(string renderpath)
        {
            try
            {
                string url = DecodificarUrlAsync(renderpath);
                string res = await apiCenService.GetXmlFile(url);
                ResponseModel responseModel = await apiCenService.UploadToFunctionAzure(res);
                if (responseModel.Content == null)
                {
                    Mensaje = "No se pudo acceder a Azure";
                    return Page();
                }
                byte[] bytes = Encoding.UTF8.GetBytes(responseModel.Content);
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
        public async Task<ActionResult> OnGetXmlDocAsync(string renderpath)
        {
            try
            {
                string url = DecodificarUrlAsync(renderpath);
                byte[] bytes = Encoding.UTF8.GetBytes(await apiCenService.GetXmlFile(url));
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
                string res = await apiCenService.GetXmlFile(url);
                ResponseModel responseModel = await apiCenService.UploadToFunctionAzure(res);
                if (responseModel.Content == null)
                {
                    Mensaje = "No se pudo acceder a Azure";
                    return Page();
                }
                //Pdf  
                byte[] bytes = await apiCenService.ConvertToPdf(responseModel.Content);
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
