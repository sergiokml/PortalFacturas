using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using PortalFacturas.Helpers;
using PortalFacturas.Models;
using PortalFacturas.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        [Authorize]
        private void Paginacion()
        {
            EmisorID = (int)TempData["EmisorID"];
            ReceptorID = (int)TempData["ReceptorID"];
            List<InstructionResult> sessionList = SessionHelperExtension.GetObjectFromJson<
                List<InstructionResult>
            >(HttpContext.Session, "Instrucciones");

            List<InstructionResult> lista = sessionList
                .OrderByDescending(
                    (InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication
                )
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            Count = sessionList.Count;
            Instructions = lista;

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

                    InstructionModel l = await apiCenService.GetInstructionsAsync(
                        EmisorID.ToString(),
                        ReceptorID.ToString()
                    );

                    Count = l.Count;
                    await apiCenService.GetDocumentos(l.Results.ToList());
                    SessionHelperExtension.SetObjectAsJson(
                        HttpContext.Session,
                        "Instrucciones",
                        l.Results
                    );

                    Instructions = l.Results
                        .OrderByDescending(
                            (InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication
                        )
                        .ToList()
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();
                    if (Instructions.Count == 0)
                    {
                        throw new Exception("No existen instrucciones de Pago.");
                    }
                }
                catch (Exception ex)
                {
                    Mensaje = ex.Message;
                }
            }
            await LlenarCombosAsync(true);
        }

        public async Task OnGetPaginaAsync()
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
                    nameof(ParticipantResult.Name)
                );

                ParticipantReceptorList = SessionHelperExtension.GetObjectFromJson<
                    List<ParticipantResult>
                >(HttpContext.Session, "ParticipantReceptor");
                ParticipantReceptor = new SelectList(
                    ParticipantReceptorList,
                    nameof(ParticipantResult.Id),
                    nameof(ParticipantResult.Name)
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
                    nameof(ParticipantResult.Name)
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
                    nameof(ParticipantResult.Name)
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
