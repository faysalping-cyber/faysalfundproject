using System.ComponentModel.DataAnnotations;

namespace FaysalFunds.API.Models
{
    public class SuccessResponse
    {        /// <summary>
             /// Gets or sets the HTTP status code of the response.
             /// </summary>
        [Range(200, 299, ErrorMessage = "Status code for success responses should be between 200 and 299.")]
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message that describes the success.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string SuccessMessage { get; set; } = "Successful";

        /// <summary>
        /// Gets or sets additional data related to the response.
        /// </summary>
        public object? Data { get; set; } = new object();
    }
}
