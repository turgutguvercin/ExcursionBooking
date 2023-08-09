using Excursion.Data;
using Excursion.Models;
using Excursion.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Excursion.Controllers
{

    public class AccountController : Controller
    {

        // Dependency injection.
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        // Display the Login page.
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        // Handle login form submission.
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {

                 // Attempt to sign in.
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        // Redirect to tour listing page after successful login.
                        return RedirectToAction("Index", "Tour");

                    }
                }
                // Password is wrong.
                TempData["Error"] = "Invalid login details.";
                return View(loginVM);

            }

            // User not found.
            TempData["Error"] = "Invalid login details.";
            return View(loginVM);
        }

        // Display the Registration page.
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        // Handle registration form submission.
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            // Check if user already exists by email.
            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            // Create user.
            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (newUserResponse.Succeeded)
            {
                // Assign role to the new user.
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

                // Show success message and prepare for JavaScript redirection.
                TempData["SuccessMessage"] = "Registration successful! Welcome. You will be redirected to login page.";
                ViewBag.JavaScriptFunction = "redirectAfterTimeout();";
            }

            return View(registerVM);

        }

        // Log the user out.
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}