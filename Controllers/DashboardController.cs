using Excursion.Interfaces;
using Excursion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Excursion.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(UserManager<AppUser> userManager, IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // Displays the user's bookings.
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (curUserId == null) return View("Error");
            var bookings = await _dashboardRepository.GetUserBookingsAsync(curUserId);

            return View(bookings);
        }


        // Shows a view to confirm the cancellation of a booking.
        [HttpGet]
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = await _dashboardRepository.GetBookingByIdAsync(id.Value);

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

            // Check if the booking belongs to the current user.
            if (booking == null || booking.AppUser.Id != currentUserId || currentUserId == null)
            {
                return Unauthorized();
            }

            return View(booking);
        }

        // Processes the cancellation of the user's booking.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _dashboardRepository.GetBookingByIdAsync(id.Value);

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

            if (booking == null || booking.AppUser.Id != currentUserId || currentUserId == null)
            {
                return Unauthorized();
            }
            var result = await _dashboardRepository.RemoveBookingAsync(booking);

            return View(booking);
        }



    }

}