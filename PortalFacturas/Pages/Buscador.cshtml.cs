using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using PortalFacturas.Helpers;
using PortalFacturas.Models;
using PortalFacturas.Services;

namespace PortalFacturas.Pages
{
    [Authorize]
    public class BuscadorModel : PageModel
    {
        private readonly IApiCenService apiCenService;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int Count { get; set; }

        public int PageSize { get; set; } = 15;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Receptor del documento")]
        public int ReceptorID { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Emisor del documento")]
        public int EmisorID { get; set; }

        [BindProperty]
        public List<InstructionResult> Instructions { get; set; } = new List<InstructionResult>();

        public SelectList ParticipantEmisor { get; set; }

        public SelectList ParticipantReceptor { get; set; }

        public List<ParticipantResult> ParticipantEmisorList { get; set; }

        public List<ParticipantResult> ParticipantReceptorList { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Folio { get; set; }

        public BuscadorModel(IApiCenService apiCenService)
        {
            this.apiCenService = apiCenService;
        }

        public async Task OnGetAsync()
        {
            //Primera carga de la página
            if (User.Identity.IsAuthenticated)
            {
                //TempData.Keep("UserName");
                await LlenarCombosAsync();
            }
        }

        public async Task<IActionResult> OnPostBuscarFolioAsync()
        {
            //Buscar Folio
            if (ModelState.IsValid && !string.IsNullOrEmpty(Folio))
            {
                List<InstructionResult> sessionList = SessionHelperExtension.GetObjectFromJson<
                    List<InstructionResult>
                >(HttpContext.Session, "Instrucciones");

                InstructionResult res = sessionList.FirstOrDefault(
                    c =>
                        c.DteResult != null
                        && c.DteResult.Any(c => c.Folio == Convert.ToInt32(Folio))
                );
                if (res == null)
                {
                    Paginacion();
                }
                else
                {
                    Count = 1;
                    Instructions.Add(res);
                }
            }
            else
            {
                Paginacion();
            }
            // Necesario!
            EmisorID = (int)TempData["EmisorID"];
            ReceptorID = (int)TempData["ReceptorID"];
            TempData.Keep("EmisorID");
            TempData.Keep("ReceptorID");

            await LlenarCombosAsync(true);
            return Page();
        }

        //[Authorize]
        private void Paginacion()
        {
            EmisorID = (int)TempData["EmisorID"];
            ReceptorID = (int)TempData["ReceptorID"];
            List<InstructionResult> sessionList = SessionHelperExtension.GetObjectFromJson<
                List<InstructionResult>
            >(HttpContext.Session, "Instrucciones");

            List<InstructionResult> lista = sessionList
                .OrderByDescending(c => c.AuxiliaryData.PaymentMatrixPublication)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            Count = sessionList.Count;
            //foreach (InstructionResult item in lista)
            //{
            //    item.DteResult = item.DteResult.OrderByDescending(c => c.EmissionDt).ToList();
            //}

            Instructions = lista;
            //foreach (InstructionResult item in Instructions)
            //{
            //    if (item.DteResult != null)
            //    {
            //        item.DteResult = item.DteResult.OrderByDescending(c => c.EmissionDt).ToList();
            //    }
            //}
            foreach (InstructionResult item in Instructions)
            {
                if (item.DteResult != null)
                {
                    item.DteResult = item.DteResult.OrderByDescending(c => c.EmissionDt).ToList();
                }
            }

            TempData.Keep("EmisorID");
            TempData.Keep("ReceptorID");
        }

        public async Task OnGetBuscarFolioAsync()
        {
            //Volver del Buscador Folios
            if (
                ModelState.IsValid && TempData["EmisorID"] != null && TempData["ReceptorID"] != null
            )
            {
                Paginacion();
            }
            await LlenarCombosAsync(true);
        }

        public async Task OnPostBuscarAsync()
        {
            //Buscador principal
            if (ModelState.IsValid)
            {
                try
                {
                    // Guardar las variables
                    TempData["EmisorID"] = EmisorID;
                    TempData["ReceptorID"] = ReceptorID;
                    TempData.Keep("EmisorID");
                    TempData.Keep("ReceptorID");

                    List<InstructionResult> l = await apiCenService.GetInstructionsAsync(
                        EmisorID.ToString(),
                        ReceptorID.ToString()
                    );

                    Count = l.Count;
                    await apiCenService.GetDocumentos(l.ToList());
                    SessionHelperExtension.SetObjectAsJson(HttpContext.Session, "Instrucciones", l);

                    Instructions = l.OrderByDescending(
                            c => c.AuxiliaryData.PaymentMatrixPublication
                        )
                        // .ToList()
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();
                    if (Instructions.Count == 0)
                    {
                        throw new Exception("No existen instrucciones de Pago.");
                    }
                    foreach (InstructionResult item in Instructions)
                    {
                        if (item.DteResult != null)
                        {
                            item.DteResult = item.DteResult
                                .OrderByDescending(c => c.EmissionDt)
                                .ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Mensaje = ex.Message;
                }
            }
            await LlenarCombosAsync(true);
        }

        public async Task OnGetPaginaAsync() // se activa al cambiar de paginas
        {
            //Páginas de Paginación
            if (
                ModelState.IsValid && TempData["EmisorID"] != null && TempData["ReceptorID"] != null
            )
            {
                Paginacion();
            }
            await LlenarCombosAsync(true);
        }

        private async Task LlenarCombosAsync(bool isPostBack = false)
        {
            if (isPostBack)
            {
                ParticipantEmisorList = SessionHelperExtension.GetObjectFromJson<
                    List<ParticipantResult>
                >(HttpContext.Session, "ParticipantEmisor");
                ParticipantEmisor = new SelectList(
                    ParticipantEmisorList,
                    nameof(ParticipantResult.Id),
                    nameof(ParticipantResult.BusinessName)
                );

                ParticipantReceptorList = SessionHelperExtension.GetObjectFromJson<
                    List<ParticipantResult>
                >(HttpContext.Session, "ParticipantReceptor");
                ParticipantReceptor = new SelectList(
                    ParticipantReceptorList,
                    nameof(ParticipantResult.Id),
                    nameof(ParticipantResult.BusinessName)
                );
            }
            else // Falso
            {
                //UserName = "miguel.buzunariz@enel.com";
                string email = User.FindFirstValue(ClaimTypes.Email);
                ParticipantReceptorList = await apiCenService.GetParticipantsAsync(email);
                ParticipantReceptor = new SelectList(
                    ParticipantReceptorList,
                    nameof(ParticipantResult.Id),
                    nameof(ParticipantResult.BusinessName)
                );
                SessionHelperExtension.SetObjectAsJson(
                    HttpContext.Session,
                    "ParticipantReceptor",
                    ParticipantReceptorList
                );

                ParticipantEmisorList = await apiCenService.GetParticipantsAsync();
                ParticipantEmisor = new SelectList(
                    ParticipantEmisorList,
                    nameof(ParticipantResult.Id),
                    nameof(ParticipantResult.BusinessName)
                );
                SessionHelperExtension.SetObjectAsJson(
                    HttpContext.Session,
                    "ParticipantEmisor",
                    ParticipantEmisorList
                );
            }
        }
    }
}
