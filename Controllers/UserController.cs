using Excursion.Models;
using Excursion.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Excursion.Controllers
{
    public class UserController : Controller
    {
        
        private readonly IUserRepository _userRepository;
        
        private readonly UserManager<AppUser> _userManager;

        public UserController(IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        // Endpoint to list all users, accessible only to the admin role
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListUsers()
        {
            var users = await _userRepository.GetAllUsersAsync(); // Fetch all users
            return View(users);
        }

        // Endpoint to display bookings of a specific user, accessible only to the admin role
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UserBookings(string id)
        {
            var bookings = await _userRepository.GetBookingsForUser(id);
            var user = await _userManager.FindByIdAsync(id); 
            ViewBag.Username = user?.UserName; // Pass the username to the view

            if (bookings == null) return View("Error"); // Handle if bookings data isn't available
            return View(bookings);
        }
    }
}
