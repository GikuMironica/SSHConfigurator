﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSHConfigurator.Models;
using SSHConfigurator.ViewModels;
using ErrorViewModel = SSHConfigurator.ViewModels.ErrorViewModel;

namespace SSHConfigurator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<THUMember> userManager;
        private readonly SignInManager<THUMember> signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<THUMember> userManager, SignInManager<THUMember> signInManager)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
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
