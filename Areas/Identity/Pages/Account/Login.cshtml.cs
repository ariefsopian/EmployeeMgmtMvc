using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeMgmtMvc.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public LoginModel(SignInManager<IdentityUser> signInManager)=>_signInManager=signInManager;

        [BindProperty] public InputModel Input { get; set; } = new();
        public class InputModel { [Required, EmailAddress] public string Email { get; set; } = string.Empty; [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty; public bool RememberMe { get; set; } }
        public void OnGet() {}
        public async Task<IActionResult> OnPostAsync(){ if(!ModelState.IsValid) return Page(); var result=await _signInManager.PasswordSignInAsync(Input.Email,Input.Password,Input.RememberMe,true); if(result.Succeeded) return LocalRedirect(Url.Content("~/")); ModelState.AddModelError(string.Empty,"Login gagal."); return Page(); }
    }
}