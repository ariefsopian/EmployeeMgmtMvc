using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeMgmtMvc.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager){_userManager=userManager;_signInManager=signInManager;}
        [BindProperty] public InputModel Input { get; set; } = new();
        public class InputModel { [Required, EmailAddress] public string Email { get; set; } = string.Empty; [Required, StringLength(100, MinimumLength=6), DataType(DataType.Password)] public string Password { get; set; } = string.Empty; [DataType(DataType.Password), Compare("Password")] public string ConfirmPassword { get; set; } = string.Empty; }
        public void OnGet(){}
        public async Task<IActionResult> OnPostAsync(){ if(!ModelState.IsValid) return Page(); var user=new IdentityUser{UserName=Input.Email,Email=Input.Email,EmailConfirmed=true}; var result=await _userManager.CreateAsync(user,Input.Password); if(result.Succeeded){ await _signInManager.SignInAsync(user,false); return LocalRedirect(Url.Content("~/")); } foreach(var e in result.Errors) ModelState.AddModelError(string.Empty,e.Description); return Page(); }
    }
}