using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PortalFacturas.Services;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortalFacturas.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApiCenService apiCenService;

        //[BindProperty]
        //public string UserName { get; set; }

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


        public void OnGet()
        {

            TempData.Clear();
        }
        public async Task<IActionResult> OnPostAsync(string UserName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Token debería ser guardado en variable cookie? !!!!!!!!!!!!!!
                    string token = await apiCenService.GetAccessTokenAsync(UserName, Password);
                    if (!string.IsNullOrEmpty(token))
                    {
                        // Login In.
                        //TempData["UserName"] = UserName;
                        await SetAuthCookieAsync(UserName);
                        return RedirectToPage("/Buscador");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Page();
        }


        private async Task SetAuthCookieAsync(string UserName)
        {
            UserName = "miguel.buzunariz@enel.com";
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Email, UserName)
            };
            ClaimsIdentity identity = new(claims, "appcookie");
            ClaimsPrincipal claimsPrincipal = new(identity);
            await HttpContext.SignInAsync("appcookie", claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = Recordar,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(1) //Dura 1 min
            });
        }


    }
}
