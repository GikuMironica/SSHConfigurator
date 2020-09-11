using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSHConfigurator.Identity;
using SSHConfigurator.Models;
using SSHConfigurator.Services;
using SSHConfigurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Controllers
{
    
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // clear the existing external cookie
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);           

            return View();
        }


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

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                                
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, _notAllowedLoginMessage);
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, _invalidLoginMessage);

            }

            return View(model);
        }


        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
