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

        [BindProperty]
        [Display(Name = "Receptor")]
        public int ReceptorID { get; set; }

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

        public async Task<List<InstructionResult>> GetPaginatedResult(string offset, int currentPage, int pageSize = 10)
        {
            InstructionModel l = await apiCenService.GetInstructionsAsync(EmisorID, ReceptorID, offset);
            Count = l.Count;

            Instructions = l.Results
                .OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication).ToList();

            await apiCenService.GetDocumentos(Instructions);

            return Instructions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
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
                //Instructions = (await apiCenService.GetInstructionsAsync(EmisorID, ReceptorID)).Results.OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication).ToList();
                //await apiCenService.GetDocumentos(Instructions);

                //Pag      
                Instructions = await GetPaginatedResult("0", CurrentPage, PageSize);

            }
            await LlenarCombosAsync();
        }

        public void OnPostPagina()
        {
            //return null;

        }

        public async Task OnGetPaginaAsync(string offset)
        {

            Instructions = await GetPaginatedResult(offset, CurrentPage, PageSize);

        }

        public async Task LlenarCombosAsync()
        {
            UserName = "miguel.buzunariz@enel.com";


            ParticipantReceptor = new SelectList(await apiCenService.GetParticipantsAsync(UserName), nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));


            ParticipantEmisor = new SelectList(await apiCenService.GetParticipantsAsync(), nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));
        }

    }

}
