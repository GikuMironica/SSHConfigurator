using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.ViewModels
{
    public class UploadKeyViewModel
    {
        [Required]
        public IFormFile KeyFile { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
