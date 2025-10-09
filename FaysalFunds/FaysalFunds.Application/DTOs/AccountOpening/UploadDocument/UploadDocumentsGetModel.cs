using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.AccountOpening.UploadDocument
{
    public class UploadDocumentsGetModel
    {
        //public byte[]? CnicUploadFront { get; set; }
        //public byte[]? CnicUploadBack { get; set; }
        //public byte[]? LivePhotoOrSelfie { get; set; }
        //public byte[]? ProofOfSourceIncome { get; set; }
        //public byte[]? PastOneYearBankstatement { get; set; }
        public List<UploadDocumentItemDTO> Body { get; set; } = new();

    }
    //public class UploadDocumentsResponseModel
    //{
    //    public List<UploadDocumentItemDTO> Body { get; set; } = new();
    //}

    public class UploadDocumentItemDTO
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public byte[]? File { get; set; }
        public string Subtext { get; set; }
    }

}
