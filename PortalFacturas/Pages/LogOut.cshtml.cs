using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Threading.Tasks;

namespace PortalFacturas.Pages
{
    public class LogOutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                //Desloguearse y re dirigir al Index
                await HttpContext.SignOutAsync("appcookie");
            }
            return Redirect("/Index");
        }
    }
}
