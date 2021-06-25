using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SSHConfigurator.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using SSHConfigurator.Domain;

namespace SSHConfigurator.Services
{
    /// <summary>
    /// This service class contains the methods for interacting with Google Recaptcha in order to block the bots.
    /// </summary>
    public class GoogleRecaptchaService : IRecaptchaService
    {
        private readonly RecaptchaSettings _recaptchaSettings;

        public GoogleRecaptchaService(IOptions<RecaptchaSettings> recaptchaSettings)
        {
            _recaptchaSettings = recaptchaSettings.Value;
        }

        public async Task<RecaptchaResponse> ReceiveVerificationAsync(string token)
        {
            var data = new GoogleReCaptchaData
            {
                Response = token,
                Secret = _recaptchaSettings.ReCaptchaSecretKey
            };

            var client = new HttpClient();

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

    public class GoogleResponse : RecaptchaResponse
    {
        public double Score { get; set; }
        public string Action { get; set; }
        public DateTime Challenge_Ts { get; set; }
        public string Hostname { get; set; }
    }
}
