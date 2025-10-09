using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class MissingStepsResponseModel
    {
        public string ProfileName { get; set; }
        public string Message { get; set; }
        public string OnboardingStatusMessage { get; set; } 
        public string Status { get; set; }
        public List<string> MissingSteps { get; set; }
    }
}
    