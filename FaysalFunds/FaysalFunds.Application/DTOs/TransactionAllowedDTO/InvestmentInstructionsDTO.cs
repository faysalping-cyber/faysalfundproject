using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{

    public class InvestmentInstructionsDTO
    {
        public string Channel { get; set; }
        public string Title { get; set; }
        public List<string> Steps { get; set; }
    }
}