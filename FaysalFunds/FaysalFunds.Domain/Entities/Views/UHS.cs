using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities.Views
{
    [Keyless]
    public class UHS
    {
        public long FOLIONO { get; set; }
        public string? DATEOFBIRTH { get; set; }
        public string? ACCOUNTCATEGORY { get; set; }
        public string? ACCOUNTNO { get; set; }
        public string? ACCOUNTTYPE { get; set; }
        public string? CNIC { get; set; }
        public string? CELLNO { get; set; }
        public string? IBAN { get; set; }
        public string? BANKNAME { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? BRANCH_CODE { get; set; }
        public string? BRANCHCITY { get; set; }
        public string? TITLE { get; set; }
        public string? FOLIOSTATUS { get; set; }
        public string? EMAILADDRESS { get; set; }
    }
}
