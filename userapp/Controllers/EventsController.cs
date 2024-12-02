using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using userapp.Models;

namespace userapp.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

      
        [Authorize] 
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,StartDate,EndDate,Description,EventType,Capacity,Country,City,Language")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartDate,EndDate,Description,EventType,Capacity,Country,City,Language")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        public IActionResult Search(string searchTerm, string category, DateTime? date)
        {
            // البحث والتصفية بناءً على القيم المدخلة
            var events = _context.Events.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                events = events.Where(e => e.Title.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(category))
            {
                events = events.Where(e => e.EventType == category);
            }

            if (date.HasValue)
            {
                events = events.Where(e => e.StartDate.Date == date.Value.Date);
            }

            
            return View(events.ToList());
        }
        public async Task<IActionResult> EventDetails(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Events/Booking/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Booking(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }
            
            // إرجاع تفاصيل الحدث إلى الصفحة
            return View(eventItem);
        }

        // POST: Events/Booking/5
        // POST: Events/Booking/5
        // POST: Events/Booking/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookTicket(int id, string fullName, string email, int numberOfTickets, string ticketType)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // الحصول على UserId من Claims
            if (eventItem == null)
            {
                return NotFound();
            }

            // تحقق من توفر التذاكر
            if (numberOfTickets > eventItem.Capacity)
            {
                ModelState.AddModelError("", "عدد التذاكر المطلوبة أكثر من المتاح.");
                return View(eventItem);
            }

            // حساب السعر الإجمالي
            var ticketPrice = ticketType == "VIP" ? eventItem.VIPPrice : eventItem.RegularPrice;
            var totalPrice = numberOfTickets * ticketPrice;

            // إنشاء الحجز وإضافة البيانات للجلسة أو TempData
            var booking = new Booking
            {
                EventId = id,
                UserId = userId,  // ربط الحجز بالمستخدم عبر UserId من Claims
                FullName = fullName,
                Email = email,
                NumberOfTickets = numberOfTickets,
                TicketType = ticketType,
                TotalPrice = totalPrice,
                BookingDate = DateTime.Now
            };

            // حفظ البيانات في TempData
            TempData["Booking"] = Newtonsoft.Json.JsonConvert.SerializeObject(booking);

            return RedirectToAction("Payment", new { id });
        }




        // POST: Events/ConfirmBooking/5
        // POST: Events/ConfirmBooking/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking()
        {
            var bookingJson = TempData["Booking"] as string;
            if (string.IsNullOrEmpty(bookingJson))
            {
                return RedirectToAction("Booking");
            }

            var booking = Newtonsoft.Json.JsonConvert.DeserializeObject<Booking>(bookingJson);

            // التحقق من توفر التذاكر مرة أخرى
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.Id == booking.EventId);
            if (eventItem == null || booking.NumberOfTickets > eventItem.Capacity)
            {
                ModelState.AddModelError("", "الحجز غير متاح حالياً.");
                return RedirectToAction("Booking", new { id = booking.EventId });
            }

            // حفظ الحجز في قاعدة البيانات
            _context.Bookings.Add(booking);
            Console.WriteLine("Booking is being saved...");
            await _context.SaveChangesAsync();
            Console.WriteLine("Booking saved successfully.");


            TempData.Remove("Booking");
            TempData["SuccessMessage"] = $"تم الحجز بنجاح! رقم الحجز: {booking.Id}, إجمالي السعر: {booking.TotalPrice}";

            return RedirectToAction("BookingConfirmation", new { id = booking.Id });
        }



        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }





        // عملية الدفع
        // عملية الدفع
        [HttpPost]
        public async Task<IActionResult> PaymentAsync(int bookingId, DeliveryInfo deliveryInfo, PaymentDetails paymentDetails)
        {
            var booking = _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefault(b => b.Id == bookingId);

            if (booking == null || booking.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) // تحقق من أن الحجز يعود للمستخدم نفسه
            {
                return NotFound(); // تأكد من وجود الحجز ومن أنه يعود للمستخدم نفسه
            }

            // تحقق من صحة البيانات المرسلة
            if (!ModelState.IsValid)
            {
                return View(); // إرجاع نفس الصفحة إذا كانت البيانات غير صحيحة
            }

            // حفظ بيانات التوصيل والدفع
            booking.DeliveryInfo = deliveryInfo;
            booking.PaymentDetails = paymentDetails;

            _context.DeliveryInfos.Add(deliveryInfo); // حفظ بيانات التوصيل
            _context.PaymentDetails.Add(paymentDetails); // حفظ بيانات الدفع

            await _context.SaveChangesAsync(); // حفظ التغييرات في قاعدة البيانات

            // التوجيه إلى صفحة التأكيد باستخدام معرف الحجز
            return RedirectToAction("BookingConfirmation", new { id = booking.Id });
        }



        [HttpPost]
        public IActionResult SubmitPaymentAndDelivery(DeliveryInfo deliveryInfo, PaymentDetails paymentDetails)
        {
            if (!ModelState.IsValid)
            {
                return View("Payment"); // If validation fails, return the view with errors.
            }

            // Save delivery and payment details to the database.
            _context.DeliveryInfos.Add(deliveryInfo);
            _context.PaymentDetails.Add(paymentDetails);

            _context.SaveChanges(); // Commit changes.

            // Redirect to confirmation page.
            return RedirectToAction("BookingConfirmation", new { id = deliveryInfo.Id });
        }


        [HttpGet]
        public IActionResult BookingConfirmation(int id)
        {
            // بدلاً من جلب بيانات الحجز من قاعدة البيانات، نعرض ببساطة الصفحة.
            return View();  // مجرد إرجاع العرض بدون التفاعل مع البيانات.
        }








    }
}

