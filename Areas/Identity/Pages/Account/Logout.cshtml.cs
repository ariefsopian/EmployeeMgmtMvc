using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeMgmtMvc.Areas.Identity.Pages.Account
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public LogoutModel(SignInManager<IdentityUser> signInManager)=>_signInManager=signInManager;
        public async Task<IActionResult> OnPost(string? returnUrl=null){ await _signInManager.SignOutAsync(); if(returnUrl!=null) return LocalRedirect(returnUrl); return RedirectToPage("/Account/Login"); }
    }
}