using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using PortalFacturas.Helpers;
using PortalFacturas.Models;
using PortalFacturas.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalFacturas.Pages
{
    public class BuscadorModel : PageModel
    {
        private readonly IApiCenService apiCenService;


        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int Count { get; set; }

        public int PageSize { get; set; } = 15;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        //[Required]
        [BindProperty(SupportsGet = true)]
        //[Display(Name = "Receptor")]
        public int ReceptorID { get; set; }

        //[Required]
        [BindProperty(SupportsGet = true)]
        //[Display(Name = "Emisor")]
        public int EmisorID { get; set; }

        [BindProperty]
        public List<InstructionResult> Instructions { get; set; } = new List<InstructionResult>();

        [TempData]
        public string UserName { get; set; }

        public SelectList ParticipantEmisor { get; set; }

        public SelectList ParticipantReceptor { get; set; }


        public List<ParticipantResult> ParticipantEmisorList { get; set; }

        public List<ParticipantResult> ParticipantReceptorList { get; set; }

        public string MensajeError { get; set; }


        public string Folio { get; set; }

        public BuscadorModel(IApiCenService apiCenService)
        {
            this.apiCenService = apiCenService;


        }

        //private async Task GetPaginatedResult(int currentPage, int pageSize = 15, bool isPostBack = false)
        //{
        //    List<InstructionResult> sessionList = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");
        //    if (sessionList == null && isPostBack == true)
        //    {
        //        InstructionModel l = await apiCenService.GetInstructionsAsync(EmisorID.ToString(), ReceptorID.ToString());
        //        Count = l.Count;
        //        await apiCenService.GetDocumentos(l.Results.ToList());
        //        SessionHelper.SetObjectAsJson(HttpContext.Session, "Instrucciones", l.Results);
        //        Instructions = l.Results.OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication).ToList().Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        //    }
        //    else
        //    {
        //        List<InstructionResult> lista = sessionList
        //            .OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication)
        //            .Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        //        Count = sessionList.Count;
        //        Instructions = lista;
        //    }
        //    //await LlenarCombosAsync(true);
        //}

        public async Task OnGetAsync()
        {
            TempData.Keep("UserName");
            await LlenarCombosAsync();
        }

        public void OnPostCarPartial(string folio)
        {
            List<InstructionResult> sessionList = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");

            Count = sessionList.Count;
            Instructions.Add(sessionList.FirstOrDefault(c => c.DteResult != null && c.DteResult.Folio == Convert.ToInt32(folio)));
        }

        public void OnGetCarPartial(string folio)
        {
            List<InstructionResult> sessionList = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");

            Count = sessionList.Count;
            Instructions.Add(sessionList.FirstOrDefault(c => c.DteResult != null && c.DteResult.Folio == Convert.ToInt32(folio)));
            //            Cars = _carService.GetAll();
            //return new PartialViewResult
            //{
            //                ViewName = "_CarPartial",
            //                ViewData = new ViewDataDictionary<List<Car>>(ViewData, Cars)
            //            };
            // return new PartialViewResult();
        }


        public async Task OnPostBuscarAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Guardar las variables
                    TempData["EmisorID"] = EmisorID;
                    TempData["ReceptorID"] = ReceptorID;
                    TempData.Keep("EmisorID");
                    TempData.Keep("ReceptorID");
                    TempData.Keep("UserName");

                    //await GetPaginatedResult(CurrentPage, PageSize, true);
                    InstructionModel l = await apiCenService.GetInstructionsAsync(EmisorID.ToString(), ReceptorID.ToString());
                    Count = l.Count;
                    await apiCenService.GetDocumentos(l.Results.ToList());
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Instrucciones", l.Results);
                    Instructions = l.Results.OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication).ToList().Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                }
                catch (Exception ex)
                {
                    MensajeError = ex.Message;
                }
            }
            await LlenarCombosAsync(true);
        }


        public async Task OnGetPaginaAsync()
        {
            if (ModelState.IsValid && TempData["EmisorID"] != null && TempData["ReceptorID"] != null)
            {
                EmisorID = (int)TempData["EmisorID"];
                ReceptorID = (int)TempData["ReceptorID"];
                //await GetPaginatedResult(CurrentPage, PageSize);
                List<InstructionResult> sessionList = SessionHelper.GetObjectFromJson<List<InstructionResult>>(HttpContext.Session, "Instrucciones");

                List<InstructionResult> lista = sessionList
                    .OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication)
                    .Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                Count = sessionList.Count;
                Instructions = lista;

                TempData.Keep("EmisorID");
                TempData.Keep("ReceptorID");
                TempData.Keep("UserName");

            }
            await LlenarCombosAsync(true);

        }

        private async Task LlenarCombosAsync(bool isPostBack = false)
        {
            if (isPostBack)
            {
                ParticipantEmisorList = SessionHelper.GetObjectFromJson<List<ParticipantResult>>(HttpContext.Session, "ParticipantEmisor");
                ParticipantEmisor = new SelectList(ParticipantEmisorList, nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));


                ParticipantReceptorList = SessionHelper.GetObjectFromJson<List<ParticipantResult>>(HttpContext.Session, "ParticipantReceptor");
                ParticipantReceptor = new SelectList(ParticipantReceptorList, nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));

            }
            else // Falso
            {
                UserName = "miguel.buzunariz@enel.com";
                ParticipantReceptorList = await apiCenService.GetParticipantsAsync(UserName);
                ParticipantReceptor = new SelectList(ParticipantReceptorList, nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ParticipantReceptor", ParticipantReceptorList);


                ParticipantEmisorList = await apiCenService.GetParticipantsAsync();
                ParticipantEmisor = new SelectList(ParticipantEmisorList, nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ParticipantEmisor", ParticipantEmisorList);
            }




        }
    }
}
