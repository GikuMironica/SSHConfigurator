using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment iHostingEnvironment;

        public HomeController(ILogger<HomeController> logger, UserManager<THUMember> userManager, SignInManager<THUMember> signInManager,
                              IKeyStorageService keyStorageService, IWebHostEnvironment IHostingEnvironment)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.keyStorageService = keyStorageService;
            iHostingEnvironment = IHostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var IsExistent = keyStorageService.IsUserExistent(User.Identity.Name);
            var UserData = new HomeViewModel
            {
                UserName = User.Identity.Name,
                HasKey = await IsExistent
            };                    

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
                // temporarily store the key in the www/temp-keys folder
                var fileName = await StoreKeyToTempLocationAsync(uploadKeyViewModel.KeyFile, User.Identity.Name);

                // call a script to delete the existing key if exists, and store the new key. 
                var result = keyStorageService.StorePublicKey(fileName, User.Identity.Name);

                // after file copied to the users .ssh folder in authorized_keys, delete key from temp folder
                if (System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);

                return RedirectToAction("Index");
            }
            return View();            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<string> StoreKeyToTempLocationAsync(IFormFile key, string username)
        {
            string uploadsFolder = Path.Combine(iHostingEnvironment.WebRootPath, "temp-keys");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + username;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //await key.CopyToAsync(new FileStream(filePath, FileMode.Create));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await key.CopyToAsync(stream);
            }

            return filePath;
        }

    }
}
