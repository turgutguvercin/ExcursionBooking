using Excursion.Interfaces;
using Excursion.Models;
using Excursion.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Excursion.Controllers
{
    public class TourController : Controller
    {
        // Dependency Injection for the tour repository
        private readonly ITourRepository _tourRepository;

        // Constructor accepting the tour repository
        public TourController(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        // Action method for the main tour listing, with an optional search term
        public async Task<IActionResult> Index(string searchTerm)
        {
            IEnumerable<Tour> tours;

            // Check if the search term is provided or not
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                 tours = await _tourRepository.GetAll();
            }
            else
            {
                tours = await _tourRepository.SearchToursAsync(searchTerm);
            }

            // Return the corresponding tours to the view
            return View(tours);
        }

        // Action method to display details of a specific tour
        public async Task<IActionResult> Detail(int id)
        {
            Tour tour = await _tourRepository.GetByIdAsync(id);
            return View(tour);
        }

        // Action method to display the tour creation form; only accessible by admins
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Action method to handle the submission of the tour creation form
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Create(CreateTourViewModel tourVM)
        {
            // Validate the model data
            if (ModelState.IsValid)
            {
                // Map the ViewModel to the Data Model
                var tour = new Tour
                {
                    Name = tourVM.Name,
                    Description = tourVM.Description,
                    Price = tourVM.Price,
                    Image = tourVM.Image,
                    Minor = tourVM.Minor,
                    Address = new Address()
                    {
                        Street = tourVM.Address.Street,
                        City = tourVM.Address.City,
                        Country = tourVM.Address.Country
                    },
                };

                // Add the tour to the repository
                _tourRepository.Add(tour);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Unsuccessful");
            }

            // If model state is invalid, return to the creation form with provided data
            return View(tourVM);
        }

        // Action method to display the tour edit form; only accessible by admins
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var tour = await _tourRepository.GetByIdAsync(id);
            if (tour == null) return View("Error");

            var tourVM = new EditTourViewModel
            {
                Name = tour.Name,
                Description = tour.Description,
                Price = tour.Price,
                Image = tour.Image,
                AddressId = tour.AddressId,
                Address = tour.Address,
                Minor = tour.Minor,
            };

            return View(tourVM);
        }

        // Action method to handle the submission of the tour edit form
        [HttpPost]
        public IActionResult Edit(int id, EditTourViewModel tourVM)
        {
            // Validate the model data
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit the excursion");
                return View("Edit", tourVM);
            }

            // Map the ViewModel to the Data Model
            var tour = new Tour
            {
                Id = id,
                Name = tourVM.Name,
                Description = tourVM.Description,
                Price = tourVM.Price,
                Image = tourVM.Image,
                Minor = tourVM.Minor,
                Address = new Address()
                {
                    Street = tourVM.Address.Street,
                    City = tourVM.Address.City,
                    Country = tourVM.Address.Country
                },
            };

            // Update the tour in the repository
            _tourRepository.Update(tour);
            return RedirectToAction("Index");
        }
    }
}
