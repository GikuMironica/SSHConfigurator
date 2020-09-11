using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SSHConfigurator.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SSHConfigurator.Services
{
    /// <summary>
    /// This service class contains the methods for interacting with Google Recaptcha in order to block the bots.
    /// </summary>
    public class GoogleRecaptchaService
    {
        private RecaptchaSettings _recaptchaSettings;

        public GoogleRecaptchaService(IOptions<RecaptchaSettings> recaptchaSettings)
        {
            _recaptchaSettings = recaptchaSettings.Value;
        }

        public virtual async Task<GoogleResponse> ReceiveVerificationAsync(string _Token)
        {
            GoogleReCaptchaData data = new GoogleReCaptchaData
            {
                Response = _Token,
                Secret = _recaptchaSettings.ReCaptchaSecretKey
            };

            HttpClient client = new HttpClient();

            var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={data.Secret}&response={data.Response}");

            var responseData = JsonConvert.DeserializeObject<GoogleResponse>(response);

            return responseData;
        }
        
    }
  

    public class GoogleReCaptchaData
    {
        public string Response { get; set; }
        public string Secret { get; set; }
    }

    public class GoogleResponse
    {
        public bool Success { get; set; }
        public double Score { get; set; }
        public string Action { get; set; }
        public DateTime Challenge_Ts { get; set; }
        public string Hostname { get; set; }
    }
}
