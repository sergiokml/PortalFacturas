using Cve.Coordinador.Extensions;
using Cve.Coordinador.Models;
using Cve.Coordinador.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using PortalFacturas.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PortalFacturas.Pages;

[Authorize]
public class BuscadorModel : PageModel
{
    private readonly IDteService dte;
    private readonly IParticipantService part;
    private readonly IAgentService age;
    private readonly IInstructionService inst;
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

    public BuscadorModel(
        IConfiguration config,
        IDteService dte,
        IParticipantService part,
        IAgentService age,
        IInstructionService inst
    )
    {
        this.inst = inst;
        this.dte = dte;
        this.age = age;
        this.config = config;
        this.part = part;
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
        List<Instruction> sessionList = SessionHelperExtension.GetObjectFromJson<List<Instruction>>(
            HttpContext.Session,
            "Instrucciones"
        );

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
                item.DteAsociados = item.DteAsociados.OrderByDescending(c => c.EmissionDt).ToList();
            }
        }

        TempData.Keep("EmisorID");
        TempData.Keep("ReceptorID");
    }

    public async Task OnGetBuscarFolioAsync()
    {
        //Volver del Buscador Folios
        if (ModelState.IsValid && TempData["EmisorID"] != null && TempData["ReceptorID"] != null)
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

                List<Instruction> l = (
                    await inst.GetById(
                        EmisorID.ToString(),
                        ReceptorID.ToString(),
                        CancellationToken.None
                    )
                )
                    .Where(c => c.Amount >= 10)
                    .ToList();

                Count = l.Count();
                await dte.GetDocumentos(l.ToList(), CancellationToken.None);
                SessionHelperExtension.SetObjectAsJson(HttpContext.Session, "Instrucciones", l);

                Instructions = l.OrderByDescending(c => c.AuxiliaryData.PaymentMatrixPublication)
                    // .ToList()
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
                if (Instructions.Count == 0)
                {
                    throw new Exception("No existen instrucciones de Pago.");
                }
                foreach (Instruction item in Instructions)
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
        //Páginas de Paginación
        if (ModelState.IsValid && TempData["EmisorID"] != null && TempData["ReceptorID"] != null)
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

            ParticipantReceptorList = SessionHelperExtension.GetObjectFromJson<List<Participant>>(
                HttpContext.Session,
                "ParticipantReceptor"
            );
            ParticipantReceptor = new SelectList(
                ParticipantReceptorList,
                nameof(Participant.Id),
                nameof(Participant.BusinessName)
            );
        }
        else
        {
            // PRIMERA CARGA
            // COMBOBOX RECEPTOR
            string email = User.FindFirstValue(ClaimTypes.Email);
            IEnumerable<Agent> agenteUser = await age.GetByEmail(email, CancellationToken.None);
            IEnumerable<Participant> receptor = await part.GetById(
                agenteUser.FirstOrDefault().Participants.Select(c => c.ParticipantID).ToArray(),
                CancellationToken.None
            );
            ParticipantReceptorList = receptor.OrderBy(c => c.BusinessName).ToList();
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
            // COMBOBOX EMISOR
            IEnumerable<Agent> agenteCve = await age.GetByEmail(
                config.GetSection("CENConfig:User").Value!,
                CancellationToken.None
            );
            IEnumerable<Participant> emisor = await part.GetById(
                agenteCve.FirstOrDefault().Participants.Select(c => c.ParticipantID).ToArray(),
                CancellationToken.None
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
