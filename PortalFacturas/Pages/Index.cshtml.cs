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
        public IndexModel(IApiCenService apiCenService)
        {
            this.apiCenService = apiCenService;
        }

        public async Task<IActionResult> OngetAsync()
        {
            try
            {
                // Des login!
                //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Verification.
                if (User.Identity.IsAuthenticated)
                {
                    // Home Page.
                    return RedirectToPage("/Index");
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //await SignInUser(UserName, false);
                    // Verification.
                    if (User.Identity.IsAuthenticated)
                    {
                        // Home Page.
                        return RedirectToPage("/Index");
                    }

                    if (await apiCenService.GetAccessTokenAsync(UserName, Password) != null)
                    {
                        // Login In.
                        TempData["UserName"] = UserName;
                        await SetAuthCookieAsync();
                        return RedirectToPage("/Buscador");
                    }

                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
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

            User.AddIdentity(identity);
            await HttpContext.SignInAsync("appcookie", claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = false
                //ExpiresUtc = DateTime.Now
            });
        }


    }
}
