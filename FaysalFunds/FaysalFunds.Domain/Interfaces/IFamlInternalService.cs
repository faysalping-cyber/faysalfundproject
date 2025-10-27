using FaysalFunds.Application.DTOs.ExternalAPI;
using FaysalFunds.Common;
using FaysalFunds.Domain.DTOs.ExternalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IFamlInternalService
    {
        Task<ApiResponseWithData<CheckBalance>> CheckCustomerBalance(CheckBalanceRequestModel request);

    }
}
