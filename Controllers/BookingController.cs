using Excursion.Interfaces;
using Excursion.Models;
using Excursion.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Excursion.Controllers
{
    // Controller responsible for booking operations.
    public class BookingController : Controller
    {
       // Dependency injection.
        private readonly IBookingRepository _bookingRepository;
        private readonly ITourRepository _tourRepository;

        
        public BookingController(IBookingRepository bookingRepository, ITourRepository tourRepository)
        {
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
        }

        // GET action for booking a tour with a given ID.
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Book(int id)
        {
            Tour tour = await _tourRepository.GetByIdAsync(id);
            if (tour == null) return View("Error");
            var bookVM = new BookViewModel
            {
                TourId = tour.Id,
                Tour = tour
            };
            return View(bookVM);
        }

        // POST action to handle the booking submission.
        [Authorize]
        [HttpPost("Booking/Book/{id}")]
        public async Task<IActionResult> Book(BookViewModel bookVM, int id)
        {
            
            var tour = await _tourRepository.GetByIdAsync(id);
            if (tour == null) return View("Error");

            // Calculate total booking cost.
            double totalCost = CalculateTotalCost(tour.Price, bookVM.NumberOfGuests);

            // Create new booking record.
            var booking = new Booking
            {
                Tour = tour,
                TourDate = bookVM.TourDate,
                NumberOfGuests = bookVM.NumberOfGuests,
                BookingNotes = bookVM.BookingNotes,
                TotalCost = totalCost,
            };

            // Attach booking to current user and save.
            var user = await _bookingRepository.GetCurrentUserAsync();
            if (user.Bookings == null)
            {
                user.Bookings = new List<Booking>();
            }
            user.Bookings.Add(booking);
            _bookingRepository.Update(user);

            // Redirect to the booking confirmation view.
            return RedirectToAction("Confirmation", new { id = booking.Id });
        }

        // Helper method to calculate the total booking cost.
        private double CalculateTotalCost(double pricePerGuest, int numberOfGuests)
        {
            return numberOfGuests * pricePerGuest;
        }

        // Confirmation view for a specific booking by its ID.
        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);

            if (booking == null) return View("Error");

            return View(booking);
        }

        // POST action to cancel a booking by its ID.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);

            if (booking == null) return View("Error");

            // Remove the booking.
            var result = await _bookingRepository.RemoveBookingAsync(booking);
            if (!result)
            {
                return View("Error");
            }

            // Redirect to the home page after successful cancellation.
            return RedirectToAction("Index", "Home");
        }
    }
}
