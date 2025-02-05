using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly LibDbContext _context;

        // Constructor to initialize the context
        public UserController(LibDbContext context)
        {
            _context = context;
        }

        // Action method to display the list of users
        public async Task<IActionResult> Index()
        {
            // Fetch all users from the database and pass to the view
            return View(await _context.Users.ToListAsync());
        }

        // Action method to show the create user form (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();  // Return the view for creating a new user
        }

        // Action method to handle user creation (POST)
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            // Check if the model is valid before adding the user
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);  // Add the new user to the database
                await _context.SaveChangesAsync();  // Save changes to the database
                return RedirectToAction("Index", "User");  // Redirect to the user list page after successful creation
            }
            return View(user);  // If the model is invalid, return to the create form with the model
        }

        // Action method to show the update user form (GET)
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            // If ID is 0, return NotFound as invalid ID
            if (id == 0) return NotFound();
            
            // Check if the user has borrowed any books
            bool hasBorrowedBooks = await _context.Borrows.AnyAsync(b => b.UserId == id);

            // If the user has borrowed books, prevent update
            if (hasBorrowedBooks)
            {
                TempData["ErrorMessage"] = "This user has borrowed books and cannot be updated.";  // Show an error message
                return RedirectToAction("Index");  // Redirect to the user list page
            }

            // Fetch the selected user to edit
            var selectedUser = await _context.Users.FindAsync(id);
            return View(selectedUser);  // Return the user to the update form
        }

        // Action method to handle user update (POST)
        [HttpPost]
        public async Task<IActionResult> Update(User updatedUser, int id)
        {
            // Check if the model is valid before updating
            if (ModelState.IsValid)
            {
                _context.Update(updatedUser);  // Update the user in the database
                await _context.SaveChangesAsync();  // Save changes to the database
                return RedirectToAction("Index", "User");  // Redirect to the user list page after successful update
            }
            return View(updatedUser);  // If the model is invalid, return to the update form with the model
        }

        // Action method to show the delete confirmation form (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            // If ID is null, return NotFound
            if (id == null) return NotFound();

            // Fetch the user to be deleted
            var user = await _context.Users.FindAsync(id);

            // If the user is not found, return NotFound
            if (user == null) return NotFound();
            
            // Check if the user has borrowed any books
            bool hasBorrowedBooks = await _context.Borrows.AnyAsync(b => b.UserId == id);

            // If the user has borrowed books, prevent deletion
            if (hasBorrowedBooks)
            {
                TempData["ErrorMessage"] = "This user has borrowed books and cannot be deleted.";  // Show an error message
                return RedirectToAction("Index");  // Redirect to the user list page
            }

            return View(user);  // Return the user to the delete confirmation form
        }

        // Action method to handle user deletion (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the user to be deleted
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);  // Remove the user from the database
                await _context.SaveChangesAsync();  // Save changes to the database
            }
            return RedirectToAction("Index");  // Redirect to the user list page after successful deletion
        }
    }
}
