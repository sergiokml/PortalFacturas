using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Interfaces;

using System.Threading.Tasks;

namespace PortalFacturas.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApiCenService apiCenService;

        public IndexModel(IApiCenService apiCenService)
        {
            this.apiCenService = apiCenService;
        }

        public async Task<IActionResult> OnPostAsync(string UserName, string Password)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (await apiCenService.GetAccessTokenAsync(UserName, Password) != null)
            {
                TempData["UserName"] = UserName;
                TempData.Keep("UserName");
                return RedirectToPage("/Buscador");
            }
            return Page();
        }
    }
}
