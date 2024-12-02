using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Migrations;
using userapp.Models;

namespace userapp.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public Users User { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public int NumberOfTickets { get; set; }
        public string TicketType { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime BookingDate { get; set; }

        public int? DeliveryInfoId { get; set; }
        public DeliveryInfo DeliveryInfo { get; set; }

        public int? PaymentDetailsId { get; set; }
        public PaymentDetails PaymentDetails { get; set; }


    }
}


