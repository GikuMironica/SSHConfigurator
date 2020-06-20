using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSHConfigurator.Identity;
using SSHConfigurator.Models;
using SSHConfigurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly LdapUserManager userManager;
        private readonly LdapSignInManager signInManager;

        public AccountController(LdapUserManager userManager, LdapSignInManager signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
               
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // clear the existing external cookie
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                                
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }


                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }

            return View(model);
        }
    }
}
