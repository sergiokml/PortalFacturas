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

        [Required]
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Receptor")]
        public string ReceptorID { get; set; }

        [Required]
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Emisor")]
        public string EmisorID { get; set; }

        [BindProperty]
        public List<InstructionResult> Instructions { get; set; } = new List<InstructionResult>();

        [TempData]
        public string UserName { get; set; }

        public SelectList ParticipantEmisor { get; set; }

        public SelectList ParticipantReceptor { get; set; }


        public string MensajeError { get; set; }

        public BuscadorModel(IApiCenService apiCenService)
        {
            this.apiCenService = apiCenService;
        }

        private async Task<List<InstructionResult>> GetPaginatedResult(int currentPage, int pageSize = 10)
        {
            //if (EmisorID == 0)
            //{
            //    throw new ArgumentNullException(nameof(EmisorID));
            //}
            //if (string.IsNullOrEmpty(ReceptorID.ToString()))
            //{
            //    throw new ArgumentNullException(nameof(ReceptorID));
            //}

            InstructionModel l = await apiCenService.GetInstructionsAsync(EmisorID, ReceptorID);
            Count = l.Count;

            Instructions = l.Results
                .OrderByDescending((InstructionResult c) => c.AuxiliaryData.PaymentMatrixPublication).ToList();
            Instructions = Instructions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            await apiCenService.GetDocumentos(Instructions);
            return Instructions;
        }

        /// <summary>
        /// Primera carga desde Index
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            await LlenarCombosAsync();
            TempData.Keep("UserName");
        }

        /// <summary>
        /// Botón Principal
        /// </summary>
        /// <returns></returns>
        public async Task OnPostBuscarAsync()
        {
            if (ModelState.IsValid)
            {

                try
                {



                    EmisorID = ModelState["EmisorID"].AttemptedValue;
                    ReceptorID = ModelState["ReceptorID"].AttemptedValue;
                    //TempData["EmisorID"] = EmisorID;
                    //TempData["ReceptorID"] = ReceptorID;


                    TempData.Keep("UserName");
                    Instructions = await GetPaginatedResult(CurrentPage, PageSize);
                    await LlenarCombosAsync();
                }
                catch (Exception ex)
                {
                    MensajeError = ex.Message;
                    //                    throw new Exception(ex.Message);
                    //return Page();
                }
            }
        }

        /// <summary>
        /// Botones Html/Xml/Pdf
        /// </summary>
        /// <returns></returns>
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

        private async Task LlenarCombosAsync()
        {
            UserName = "miguel.buzunariz@enel.com";
            ParticipantReceptor = new SelectList(await apiCenService.GetParticipantsAsync(UserName), nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));
            ParticipantEmisor = new SelectList(await apiCenService.GetParticipantsAsync(), nameof(ParticipantResult.Id), nameof(ParticipantResult.Name));

        }
    }
}
