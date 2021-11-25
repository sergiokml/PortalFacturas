using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Interfaces;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortalFacturas.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApiCenService apiCenService;

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public bool Recordar
        {
            get;
            set;
        }

        public IndexModel(IApiCenService apiCenService)
        {
            this.apiCenService = apiCenService;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await apiCenService.GetAccessTokenAsync(UserName, Password) != null)
                    {
                        // Login In.
                        TempData["UserName"] = UserName;
                        //await SetAuthCookieAsync();
                        return RedirectToPage("/Buscador");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return Page();
        }

        private async Task SetAuthCookieAsync()
        {
            List<Claim> claims = new List<Claim> {
                 new Claim(ClaimTypes.Email, UserName),
                 new Claim(ClaimTypes.UserData, Password)
            };
            ClaimsIdentity identity = new(claims, "appcookie");
            ClaimsPrincipal claimsPrincipal = new(identity);
            await HttpContext.SignInAsync("appcookie", claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = Recordar
                //ExpiresUtc = DateTime.Now
            });
        }


    }
}
