namespace userapp.Models
{
    public class Event
    {
        public int Id { get; set; } 
        public string Title { get; set; } 
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public string Description { get; set; } 
        public string EventType { get; set; } 
        public int? Capacity { get; set; } 
        public string Country { get; set; } 
        public string City { get; set; } 
        public string Language { get; set; }
        public decimal RegularPrice { get; set; } = 100; 
        public decimal VIPPrice { get; set; } = 250;
    }

}
