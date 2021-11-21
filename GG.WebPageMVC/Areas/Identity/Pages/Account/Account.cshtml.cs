using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GG.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace GG.WebPageMVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class AccountModel : PageModel
    {

        private readonly SignInManager<PrivateUser> _signInManager;
        private readonly UserManager<PrivateUser> _userManager;
        private readonly ILogger<AccountModel> _logger;
        private readonly IEmailSender _emailSender;

        public AccountModel(SignInManager<PrivateUser> signInManager, UserManager<PrivateUser> userManager, ILogger<AccountModel> logger, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            ExternalLogins = (_signInManager.GetExternalAuthenticationSchemesAsync()).Result.ToList();
        }

        [BindProperty]
        public InputModelSignup InputRegister { get; set; }

        [BindProperty]
        public InputModelLogin InputLogin { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostRegisterAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid<InputModelSignup>())
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            {
                var user = new PrivateUser { UserName = InputRegister.REmail, Email = InputRegister.REmail };
                var result = await _userManager.CreateAsync(user, InputRegister.RPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(InputRegister.REmail, "Confirma tu Email",
                        $"Porfavor confirma tu cuenta con el siguiente enlace <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click aqui</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = InputRegister.REmail, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid<InputModelLogin>())
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
             
                var result = await _signInManager.PasswordSignInAsync(InputLogin.LEmail, InputLogin.LPassword, InputLogin.LRememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = InputLogin.LRememberMe });
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





    }

    public static class ValidationsExtension
    {
        public static bool IsValid<T>(this ModelStateDictionary dictionary)
        {
            var properties = typeof(T).GetProperties().Select(p => p.Name);
            List<ModelStateEntry> validations = new List<ModelStateEntry>();
            bool check = true;
           
            foreach (var item in properties)
            {

                var result = dictionary.Where(s => s.Key.Contains(item)).FirstOrDefault().Value;

                if (result != null)
                {
                    check &= result.Errors.Count == 0;
                }

            }

            return check;
        }
    }


    public class InputModelSignup
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string REmail { get; set; }

        [Required]
        [StringLength(64)]
        [Display(Name = "Nombre")]
        public string RName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string RPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("RPassword", ErrorMessage = "La contraseña y la contraseña de validacion no son iguales!")]
        public string RConfirmPassword { get; set; }
    }

    public class InputModelLogin
    {
        [Required]
        [EmailAddress]
        [Display(Name = "LEmail")]
        public string LEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "LPassword")]
        public string LPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool LRememberMe { get; set; }
    }


   

}
