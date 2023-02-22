using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Cve.Coordinador;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PortalFacturas.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CoordinadorInit cen;

        //[BindProperty]
        //public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public bool Recordar { get; set; }

        public IndexModel(CoordinadorInit cen)
        {
            this.cen = cen;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Buscador");
            }
            TempData.Clear();
            //throw new Exception();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string UserName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string token = await cen.AuthenticateService.AuthenticateAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        await SetAuthCookieAsync(UserName);
                        return RedirectToPage("/Buscador");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UserName) && Password == "cvepagos2022")
                        {
                            //UserName = "miguel.nava@goplicity.com";
                            await SetAuthCookieAsync(UserName);
                            return RedirectToPage("/Buscador");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToPage("/Error");
                //throw new Exception(ex.Message);
            }
            return Page();
        }

        private async Task SetAuthCookieAsync(string UserName)
        {
            List<Claim> claims = new() { new Claim(ClaimTypes.Email, UserName) };
            ClaimsIdentity identity = new(claims, "appcookie");
            ClaimsPrincipal claimsPrincipal = new(identity);
            await HttpContext.SignInAsync(
                "appcookie",
                claimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = Recordar,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(120) //Dura 120 min
                }
            );
        }
    }
}
