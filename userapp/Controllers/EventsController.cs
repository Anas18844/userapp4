﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> BookTicket(int id, string fullName, string email, int numberOfTickets, string ticketType)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            var ticketPrice = ticketType == "VIP" ? 100 : 250;
            var totalPrice = numberOfTickets * ticketPrice;

            // إنشاء الحجز
            var booking = new Booking
            {
                EventId = id,
                UserId = userId, // تعيين معرف المستخدم
                FullName = fullName,
                Email = email,
                NumberOfTickets = numberOfTickets,
                TicketType = ticketType,
                TotalPrice = totalPrice,
                BookingDate = DateTime.Now
            };

            // إضافة الحجز إلى قاعدة البيانات
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"تم الحجز بنجاح! رقم الحجز: {booking.Id}, إجمالي السعر: {totalPrice}";

            return RedirectToAction("BookingConfirmation", new { id = booking.Id });
        }


        // POST: Events/ConfirmBooking/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(int id, string cardDetails, string paypalDetails, string address, int numberOfTickets, string ticketType, string? userId)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            

            // تحقق من تفاصيل الدفع
           

            // حساب السعر الإجمالي
            var totalPrice = numberOfTickets * (ticketType == "VIP" ? 100 : 50);

            // إضافة الحجز إلى النظام أو قاعدة البيانات
            var booking = new Booking
            {
                EventId = eventItem.Id,
                UserId = userId,
                NumberOfTickets = numberOfTickets,
                TicketType = ticketType,
                TotalPrice = totalPrice,
                BookingDate = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"تم الحجز بنجاح! رقم الحجز: {booking.Id}, إجمالي السعر: {totalPrice}";

            return RedirectToAction("BookingConfirmation", new { id = booking.Id });
        }


        // GET: Events/BookingConfirmation/5
        public async Task<IActionResult> BookingConfirmation(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        public ActionResult Payment()
        {
            return View();
        }

    }
}

