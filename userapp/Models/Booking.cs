using userapp.Models;

public class Booking
{
    public int Id { get; set; } // رقم الحجز

    public int EventId { get; set; } // الحدث المرتبط بالحجز
    public Event Event { get; set; } // ربط الحدث

    public string UserId { get; set; } // معرف المستخدم (المستفيد من الحجز)
    public Users User { get; set; } // ربط المستخدم (يجب أن يكون لديك كلاس ApplicationUser للمستخدمين)

    public string FullName { get; set; } // اسم المستخدم الذي قام بالحجز
    public string Email { get; set; } // البريد الإلكتروني لتأكيد الحجز

    public int NumberOfTickets { get; set; } // عدد التذاكر
    public string TicketType { get; set; } // نوع التذكرة (VIP، عادية)
    public decimal TotalPrice { get; set; } // السعر الإجمالي

    public DateTime BookingDate { get; set; } // تاريخ الحجز

}
