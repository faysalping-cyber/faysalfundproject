using System.ComponentModel.DataAnnotations;

namespace FaysalFundsWrapper.Models
{
    public class SuccessResponse<T>
    {
        public int StatusCode { get; set; }
        [Required]
        [StringLength(500)]
        public string SuccessMessage { get; set; } = "Successful";
        public T Data { get; set; } 
    }
}
