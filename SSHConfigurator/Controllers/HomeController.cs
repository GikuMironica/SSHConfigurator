using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSHConfigurator.Models;
using SSHConfigurator.Services;
using SSHConfigurator.ViewModels;
using ErrorViewModel = SSHConfigurator.ViewModels.ErrorViewModel;

namespace SSHConfigurator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<THUMember> userManager;
        private readonly SignInManager<THUMember> signInManager;
        private readonly IKeyStorageService keyStorageService;

        public HomeController(ILogger<HomeController> logger, UserManager<THUMember> userManager, SignInManager<THUMember> signInManager,
                              IKeyStorageService keyStorageService)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.keyStorageService = keyStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var IsExistent = keyStorageService.IsUserExistent(User.Identity.Name);
            var UserData = new HomeViewModel
            {
                UserName = User.Identity.Name,
                HasKey = IsExistent
            };

            /**
             * 1. Access File system
             * 2. Check if folder with username exists
             * 3. Check if pub key exists in it
             * 4. Parse pub key name, display the name ( pass through viewmodel )
             */

            return View(UserData);
        }
            
        [HttpPost]
        public async Task<IActionResult> DeleteKey(string name)
        {
            /*
             * 1. Access file system
             * 2. Delete folder + pub key
             */
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UploadKey()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadKey(UploadKeyViewModel uploadKeyViewModel)
        {
            if (ModelState.IsValid)
            {
                /**
                 * 1. Access File system
                 * 2. Check if folder exist / Create
                 * 3. If pub key exists, delete
                 * 4. Store the new pub key
                 */
                return RedirectToAction("Index");
            }
            return View();            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
