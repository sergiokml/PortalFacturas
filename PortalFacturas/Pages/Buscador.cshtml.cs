using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using PortalFacturas.Interfaces;
using PortalFacturas.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalFacturas.Pages
{
    public class BuscadorModel : PageModel
    {
        private readonly IApiCenService apiCenService;

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

        public async Task OnGetAsync()
        {
            await LlenarCombosAsync();
            TempData.Keep("UserName");
        }

        public async Task OnPostBuscarAsync()
        {
            if (ModelState.IsValid)
            {
                Instructions = (await apiCenService.GetInstructionsAsync(EmisorID, ReceptorID)).Results.OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication).ToList();
                await apiCenService.GetDocumentos(Instructions);
            }
            await LlenarCombosAsync();
        }

        public async Task LlenarCombosAsync()
        {
            UserName = "miguel.buzunariz@enel.com";


            ParticipantReceptor = new SelectList(await apiCenService.GetParticipantsAsync(UserName), nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));


            ParticipantEmisor = new SelectList(await apiCenService.GetParticipantsAsync(), nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));
        }

    }

}
