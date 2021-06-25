using System.ComponentModel.DataAnnotations;

namespace SSHConfigurator.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
