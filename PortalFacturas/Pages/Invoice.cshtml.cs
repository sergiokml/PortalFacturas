
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Helpers;
using PortalFacturas.Models;
using PortalFacturas.Services;

using System;
using System.Collections.Generic;
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
                                if (!string.IsNullOrEmpty(dte.EmissionErpA))
                                {
                                    string EmissionErpB = dte.EmissionErpA;
                                    byte[] bytes = await sharePointService
                                        .DownloadConvertedFileAsync(EmissionErpB);
                                    FileResult fileResult = new FileContentResult(bytes, "text/html")
                                    {
                                        FileDownloadName = $"{dte.Folio}.xml"
                                    };
                                    return fileResult;
                                }
                            }
                        }
                    }
                }

                //foreach (InstructionResult item in ejemplo)
                //{
                //    if (item != null && item.DteResult != null && item.DteResult.Count > 0)
                //    {
                //        DteResult t = item.DteResult.FirstOrDefault(c => c.Id == render);
                //    }
                //}

                ////var x = ejemplo.FirstOrDefault(c=>c.DteResult.FirstOrDefault(f =>f.Id == render));
                //InstructionResult instruction = ejemplo.FirstOrDefault(c => c.Id == render);

                //if (instruction.DteResult[0].EmissionErpA != null)
                //{
                //    string EmissionErpB = instruction.DteResult[0].EmissionErpA;
                //    byte[] bytes = await sharePointService
                //        .DownloadConvertedFileAsync(EmissionErpB);
                //    FileResult fileResult = new FileContentResult(bytes, "text/html")
                //    {
                //        FileDownloadName = $"{instruction.DteResult[0].Folio}.xml"
                //    };
                //    return fileResult;
                //}





                //byte[] bytes = await sharePointService
                //          .DownloadConvertedFileAsync(render);
                //MemoryStream memoryStream = new(bytes);
                //return new FileStreamResult(memoryStream, "text/html");
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            //Mensaje = "No existe ID 'EmissionErpA'";
            Mensaje = "No se puede mostrar el doc.";
            return Page();
        }

        //XmlDoc
        public async Task<ActionResult> OnGetXmlDocAsync(int render)
        {
            try
            {
                List<InstructionResult> ejemplo = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
                InstructionResult instruction = ejemplo.FirstOrDefault(c => c.Id == render);

                if (instruction.DteResult[0].EmissionErpB != null)
                {
                    string EmissionErpB = instruction.DteResult[0].EmissionErpB;
                    byte[] bytes = await sharePointService
                        .DownloadConvertedFileAsync(EmissionErpB);
                    FileResult fileResult = new FileContentResult(bytes, "application/xml")
                    {
                        FileDownloadName = $"{instruction.DteResult[0].Folio}.xml"
                    };
                    return fileResult;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            //Mensaje = "No existe ID 'EmissionErpB'";
            Mensaje = "No se puede mostrar el doc.";
            return Page();
        }


        public async Task<ActionResult> OnGetPdfDocAsync(int render)
        {
            try
            {
                List<InstructionResult> ejemplo = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
                InstructionResult instruction = ejemplo.FirstOrDefault(c => c.Id == render);

                if (instruction.DteResult[0].ReceptionErp != null)
                {
                    string ReceptionErp = instruction.DteResult[0].ReceptionErp;
                    byte[] doc = await sharePointService
                        .DownloadConvertedFileAsync(ReceptionErp);
                    byte[] bytes = await convertToPdfService
                        .ConvertToPdf(Encoding.UTF8.GetString(doc));
                    FileResult fileResult = new FileContentResult(bytes, "application/pdf")
                    {
                        FileDownloadName = $"{instruction.DteResult[0].Folio}.pdf"
                    };
                    return fileResult;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            //Mensaje = "No existe ID 'ReceptionErp'";
            Mensaje = "No se puede mostrar el doc.";
            return Page();
        }
    }
}
