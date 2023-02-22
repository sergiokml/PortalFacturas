using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Cve.Coordinador;
using Cve.Coordinador.Extensions;
using Cve.Coordinador.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

using PortalFacturas.Helpers;

namespace PortalFacturas.Pages
{
    [Authorize]
    public class BuscadorModel : PageModel
    {
        private readonly CoordinadorInit cen;
        private readonly IConfiguration config;

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
        public List<Instruction> Instructions { get; set; } = new List<Instruction>();

        public SelectList ParticipantEmisor { get; set; }

        public SelectList ParticipantReceptor { get; set; }

        public List<Participant> ParticipantEmisorList { get; set; }

        public List<Participant> ParticipantReceptorList { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Folio { get; set; }

        public BuscadorModel(CoordinadorInit cen, IConfiguration config)
        {
            this.cen = cen;
            this.config = config;
        }

        public async Task OnGetAsync()
        {
            //Primera carga de la p�gina
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
                List<Instruction> sessionList = SessionHelperExtension.GetObjectFromJson<
                    List<Instruction>
                >(HttpContext.Session, "Instrucciones");

                Instruction res = sessionList.FirstOrDefault(
                    c =>
                        c.DteAsociados != null
                        && c.DteAsociados.Any(c => c.Folio == Convert.ToInt32(Folio))
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
            List<Instruction> sessionList = SessionHelperExtension.GetObjectFromJson<
                List<Instruction>
            >(HttpContext.Session, "Instrucciones");

            List<Instruction> lista = sessionList
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
            foreach (Instruction item in Instructions)
            {
                if (item.DteAsociados != null)
                {
                    item.DteAsociados = item.DteAsociados
                        .OrderByDescending(c => c.EmissionDt)
                        .ToList();
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

                    var l = (
                        await cen.InstructionService.GetManyById(
                            EmisorID.ToString(),
                            ReceptorID.ToString()
                        )
                    )
                        .Where(c => c.Amount >= 10)
                        .ToList();

                    Count = l.Count();
                    await cen.DteService.GetDocumentos(l.ToList());
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
                    foreach (var item in Instructions)
                    {
                        if (item.DteAsociados != null)
                        {
                            item.DteAsociados = item.DteAsociados
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
            //P�ginas de Paginaci�n
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
                // CUANDO YA HE SELECCIONADO AMBOS PARTICIPANTES EN LOS CBOS
                ParticipantEmisorList = SessionHelperExtension.GetObjectFromJson<List<Participant>>(
                    HttpContext.Session,
                    "ParticipantEmisor"
                );
                ParticipantEmisor = new SelectList(
                    ParticipantEmisorList,
                    nameof(Participant.Id),
                    nameof(Participant.BusinessName)
                );

                ParticipantReceptorList = SessionHelperExtension.GetObjectFromJson<
                    List<Participant>
                >(HttpContext.Session, "ParticipantReceptor");
                ParticipantReceptor = new SelectList(
                    ParticipantReceptorList,
                    nameof(Participant.Id),
                    nameof(Participant.BusinessName)
                );
            }
            else // Falso
            {
                string email = User.FindFirstValue(ClaimTypes.Email);
                var agenteUser = await cen.AgentService.GetByEmail(email);
                var receptor = await cen.ParticipantService.GetManyAsync(
                    agenteUser.Participants.Select(c => c.ParticipantID).ToArray()
                );
                ParticipantReceptorList = receptor.ToList();
                ParticipantReceptor = new SelectList(
                    ParticipantReceptorList,
                    nameof(Participant.Id),
                    nameof(Participant.BusinessName)
                );
                SessionHelperExtension.SetObjectAsJson(
                    HttpContext.Session,
                    "ParticipantReceptor",
                    ParticipantReceptorList
                );
                // EMAIL PROPIO
                var agenteCve = await cen.AgentService.GetByEmail(
                    config.GetSection("CENConfig:User").Value!
                );
                var emisor = await cen.ParticipantService.GetManyAsync(
                    agenteCve.Participants.Select(c => c.ParticipantID).ToArray()
                );
                ParticipantEmisorList = emisor.OrderBy(c => c.BusinessName).ToList();
                ParticipantEmisor = new SelectList(
                    ParticipantEmisorList,
                    nameof(Participant.Id),
                    nameof(Participant.BusinessName)
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
