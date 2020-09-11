using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Options
{
    /// <summary>
    /// This is a configuration class containing the credentials for the Google Recaptcha service.
    /// Can be injected with Dependency Injection in the required services.
    /// </summary>
    public class RecaptchaSettings
    {
        public string ReCaptchaSiteKey { get; set; }
        public string ReCaptchaSecretKey { get; set; }
    }
}
