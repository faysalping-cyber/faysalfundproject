using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.TransactionAllowed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface ITransactionFeatureRepository
    {
        Task<List<TransactionFeatures>> GetAllFeatures();
        Task<TransactionFeatures> GetTransactionFeatureById(long TransactionFeatureID);


    }
}
