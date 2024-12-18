﻿using Microsoft.AspNetCore.Mvc;

namespace userapp.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string CVV { get; set; }
        public DateTime PaymentDate { get; set; }
    }

}
