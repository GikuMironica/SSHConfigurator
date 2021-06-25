using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
