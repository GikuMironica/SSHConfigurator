using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Controllers
{
    /// <summary>
    /// This controller acts as a global exception handler.
    /// After an unhandled error occurs in the system, 
    /// the middleware redirects the request to an appropriate endpoint of this controller.
    /// </summary>
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }


        /// <summary>
        /// This endpoint handles the HTTP 404 error, and returns the appropriate view with a message to the user.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    //logger.LogWarning($"404 Error Occured. Path = {statusCodeResult.OriginalPath}" + $"and QueryString ={statusCodeResult.OriginalQueryString}");
                    break;

            }

            return View("NotFound");
        }

        /// <summary>
        /// This endpoint handles the errors and returns a view with an appropriate message to the user.
        /// </summary>
        /// <returns></returns>
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            // logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error} ");
            return View("Error");
        }
    }
}
