using FaysalFundsInternal.Application.DTOs;
using FaysalFundsInternal.Common;
using FaysalFundsInternal.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsInternal.Application.Services
{
    public class KuickPayService
    {
        private readonly Constants _constants;
        private readonly IAPICallService _aPICallService;
        public KuickPayService(IOptions<Constants> options, IAPICallService aPICallService)
        {
         
            _constants = options.Value;
            _aPICallService = aPICallService;


        }
        public async Task<KuickPayResponseModel> GenerateKuickPayID(GenerateKuickPayIdDTO model)
        {
            //decimal fee = await CalculateKuickPayFee(model.Amount);



            var headers = new Dictionary<string, string>()
            {
                ["username"] = _constants.KuickPayUserName,
                ["password"] = _constants.KuickPayPassword
            };
            KuickPayRequestModel kuickPayRequestModel = new()
            {
                institutionID = _constants.InstitutionId,
                registrationNumber = model.FolioNo,
                Head1 = "Investment Amount",
                Amount1 = 100,
                Head2 = "KuickPay Charges",
                Amount2 = 70,
                TotalAmount = 170,
                DueDate = DateTime.Now.ToString("yyyy-MM-dd"),
                AmountAfterDueDate = 100,
                ExpiryDate = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"),
                IssueDate = DateTime.Now.ToString("yyyy-MM-dd"),
                VoucherMonth = DateTime.Now.ToString("MM"),
                VoucherYear = DateTime.Now.ToString("yyyy"),
                Name = "abcd",
                Mobile = model.Mobile,
                Email = model.Email,
                Branch = "dummy"
            };
            var response = await _aPICallService.PostAsync<KuickPayRequestModel, KuickPayResponseModel>("https://uat-adminapi.kuickpay.com/api/insertPPMVoucher", kuickPayRequestModel, headers);
            //KuickPayResponseModel kuickPayModel = JsonConvert.DeserializeObject<KuickPayResponseModel>(response);
            return response;
        }
    }
}
