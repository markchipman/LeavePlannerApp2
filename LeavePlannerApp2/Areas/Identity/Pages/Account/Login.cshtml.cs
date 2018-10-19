using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using LeavePlannerApp2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LeavePlannerApp2.Data;

namespace LeavePlannerApp2.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<MyUserStore> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<MyUserStore> _userManager;
        private readonly ApplicationDbContext _context;

        public LoginModel(SignInManager<MyUserStore> signInManager, ILogger<LoginModel> logger, UserManager<MyUserStore> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }   

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                
                //var user = _userManager.;
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                //if (User.IsInRole(MyRoles.Admin) && result.Succeeded)
                //{
                //    _logger.LogInformation("Admin logged in.");
                //    return LocalRedirect("~/Home/AdminDashboard");
                //}

                
                
                if (result.Succeeded)
                {
                    //var user = _context.Users.FirstOrDefault(x => x.Email == Input.Email);

                    //_logger.LogInformation("User logged in.");
                    //return RedirectToAction("CustomDashboard", new { username = user.UserName});

                    if (User.IsInRole(MyRoles.Admin))
                    {
                        return LocalRedirect("~/Home/AdminDashboard");
                    }
                    else
                    {
                        return LocalRedirect("~/Employees/Dashboard");
                    }
                   
                   // return LocalRedirect("~/Employees/Dashboard");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public IActionResult CustomDashboard(string username)
        {
            if (User.IsInRole(MyRoles.Admin))
            {
                return RedirectToAction("AdminDashboar","Home");
            }
            return LocalRedirect("~/Employees/Dashboard");
        }
    }
}
