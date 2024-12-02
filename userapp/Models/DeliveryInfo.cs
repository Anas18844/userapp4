using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace userapp.Models
{
    public class DeliveryInfo
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required, RegularExpression(@"\d{5}")]
        public string ZipCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }


}
