using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSHConfigurator.Identity;
using SSHConfigurator.Services;
using SSHConfigurator.ViewModels;
using System.Threading.Tasks;

namespace SSHConfigurator.Controllers
{
    /// <summary>
    /// This controller handles the authentication and authorization process. 
    /// </summary>
    public class AccountController : Controller
    {
        private readonly LdapSignInManager signInManager;
        private readonly GoogleRecaptchaService recaptchaService;
        private readonly string _notAllowedLoginMessage = "You must be enrolled in a specific course to access this platform";
        private readonly string _invalidLoginMessage = "Invalid Login Attempt";

        public AccountController( LdapSignInManager signInManager, GoogleRecaptchaService recaptchaService)
        {
            this.signInManager = signInManager;
            this.recaptchaService = recaptchaService;
        }


        /// <summary>
        /// This endpoint returns the view with the login page.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // clear the existing external cookie
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);           

            return View();
        }


        /// <summary>
        /// This endpoint processes the uploaded form with the user's credentials.
        /// As result, the user is either logged in and redirected to the home page or rejected.
        /// </summary>
        /// <param name="model">Contains the user's credentials and the Google Recaptcha token.</param>
        /// <param name="returnUrl">Contains the initially requested page's URL</param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            // Google Recaptcha Verification
            var googleRecaptcha = await recaptchaService.ReceiveVerificationAsync(model.Token);

            if (!googleRecaptcha.Success)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                                
                if (result.Succeeded)
                {
                    // the following if else block prevents Open-Redirect Attacks.
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "home");
                    }
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, _notAllowedLoginMessage);
                    return View(model);
                }

            ModelState.AddModelError(string.Empty, _invalidLoginMessage);
            return View(model);
        }


        /// <summary>
        /// This endpoint logs the user out of the system.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
