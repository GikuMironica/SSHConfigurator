using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSHConfigurator.Domain;

namespace SSHConfigurator.Services
{
    public interface IRecaptchaService
    { 
        Task<RecaptchaResponse> ReceiveVerificationAsync(string Token);
    }
}
