using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace userapp.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }
        [Required]
        public string CardHolder { get; set; }
        [Required, CreditCard]
        public string CardNumber { get; set; }
        [Required]
        public string ExpMonth { get; set; }
        [Required]
        public string ExpYear { get; set; }
        [Required, RegularExpression(@"\d{3}")]
        public string CVV { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }


}
