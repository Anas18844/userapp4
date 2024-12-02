using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using userapp.Models;

public class EventsDbContext : DbContext
{
    public EventsDbContext(DbContextOptions<EventsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<DeliveryInfo> DeliveryInfos { get; set; }
    public DbSet<PaymentDetails> PaymentDetails { get; set; }

    public DbSet<Users>Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // تأكيد وجود العلاقة بين جدول Bookings و Users
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)      // الحجز مرتبط بمستخدم واحد
            .WithMany()               // يمكن أن يكون لدى المستخدم العديد من الحجوزات
            .HasForeignKey(b => b.UserId)  // UserId هو المفتاح الخارجي
            .OnDelete(DeleteBehavior.Cascade);  // (اختياري) عند حذف المستخدم، يتم حذف الحجوزات المرتبطة به
    }

}
