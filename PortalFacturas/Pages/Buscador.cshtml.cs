using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using PortalFacturas.Interfaces;
using PortalFacturas.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        [TempData]
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Receptor")]
        public int ReceptorID { get; set; }

        [TempData]
        [BindProperty]
        [Display(Name = "Emisor")]
        public int EmisorID { get; set; }

        [BindProperty]
        public List<InstructionResult> Instructions { get; set; } = new List<InstructionResult>();

        [TempData]
        public string UserName { get; set; }

        public SelectList ParticipantEmisor { get; set; }

        public SelectList ParticipantReceptor { get; set; }

        public BuscadorModel(IApiCenService apiCenService)
        {
            this.apiCenService = apiCenService;
        }

        public async Task<List<InstructionResult>> GetPaginatedResult(int currentPage, int pageSize = 10)
        {
            if (EmisorID is 0)
            {
                throw new ArgumentNullException(nameof(EmisorID));
            }
            if (ReceptorID is 0)
            {
                throw new ArgumentNullException(nameof(ReceptorID));
            }



            InstructionModel l = await apiCenService.GetInstructionsAsync(EmisorID, ReceptorID);
            Count = l.Count;

            Instructions = l.Results
                .OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication).ToList();
            Instructions = Instructions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            await apiCenService.GetDocumentos(Instructions);
            return Instructions;
        }

        public async Task OnGetAsync()
        {
            await LlenarCombosAsync();
            TempData.Keep("UserName");
        }

        public async Task OnPostBuscarAsync()
        {
            if (ModelState.IsValid)
            {
                Instructions = await GetPaginatedResult(CurrentPage, PageSize);
                TempData["EmisorID"] = EmisorID;
                TempData["ReceptorID"] = ReceptorID;
            }
            await LlenarCombosAsync();
        }

        public async Task OnGetPaginaAsync()
        {
            if (ModelState.IsValid)
            {
                Instructions = await GetPaginatedResult(CurrentPage, PageSize);
                TempData.Keep("EmisorID");
                TempData.Keep("ReceptorID");
                await LlenarCombosAsync();
            }
        }

        public async Task LlenarCombosAsync()
        {
            UserName = "miguel.buzunariz@enel.com";
            ParticipantReceptor = new SelectList(await apiCenService.GetParticipantsAsync(UserName), nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));
            ParticipantEmisor = new SelectList(await apiCenService.GetParticipantsAsync(), nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));
        }
    }
}
