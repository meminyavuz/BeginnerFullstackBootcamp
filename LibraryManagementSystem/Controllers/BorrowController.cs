using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowController : Controller
    {
        private readonly LibDbContext _context;

        // Constructor to initialize the context
        public BorrowController(LibDbContext context)
        {
            _context = context;
        }

        // Action method to display the list of borrowed books
        public async Task<IActionResult> Index()
        {
            // Fetch all borrow records along with related user and book information
            var borrowList = await _context.Borrows
                .Include(b => b.User)  // Include user information
                .Include(b => b.Book)  // Include book information
                .ToListAsync();

            return View(borrowList);  // Return the borrow list to the view
        }

        // Action method to show the borrow form for a specific user (GET)
        [HttpGet]
        public async Task<IActionResult> Borrow(int id)
        {
            // Fetch available books that can be borrowed
            var availableBooks = await _context.Books
                .Where(b => b.IsEnable)
                .ToListAsync();

            // Fetch books already borrowed by the user
            var borrowedBooks = await _context.Borrows
                .Where(b => b.UserId == id)
                .Include(b => b.Book)  // Include book information
                .ToListAsync();

            // Create the model to pass to the view
            var model = new BorrowViewModel
            {
                UserId = id,
                AvailableBooks = availableBooks,  // List of available books
                BorrowedBooks = borrowedBooks     // List of borrowed books by the user
            };

            return View(model);  // Return the borrow view with the model
        }

        // Action method to handle book borrowing (POST)
        [HttpPost]
        public async Task<IActionResult> Borrow(int userId, int bookId)
        {
            // Fetch the book to be borrowed
            var book = await _context.Books.FindAsync(bookId);

            // If book is not found or not available, return NotFound
            if (book == null || !book.IsEnable)
            {
                return NotFound();
            }

            // Mark the book as borrowed by disabling it
            book.IsEnable = false;

            // Create a new borrow record
            var borrow = new Borrow
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = DateTime.Now  // Set the borrow date to the current date and time
            };

            // Add the borrow record to the context and save changes
            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();

            // Redirect to the user list page after borrowing
            return RedirectToAction("Index", "User");
        }

        // Action method to show the return form for a borrowed book (GET)
        [HttpGet]
        public async Task<IActionResult> Return(int? id)
        {
            // If ID is null, return NotFound
            if (id == null) return NotFound();

            // Fetch the borrow record including the book information
            var borrow = await _context.Borrows
                                 .Include(b => b.Book)  // Ensure the related book information is loaded
                                 .FirstOrDefaultAsync(b => b.Id == id);

            // If the borrow record or book is not found, return NotFound
            if (borrow == null || borrow.Book == null)
            {
                return NotFound();
            }

            // Return the borrow record to the view for returning the book
            return View(borrow);
        }

        // Action method to handle book return (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            // Fetch the borrow record along with book information
            var borrow = await _context.Borrows
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.Id == id);

            // If the borrow record is not found, return NotFound
            if (borrow == null) return NotFound();

            // If the book is not found, return NotFound
            if (borrow.Book == null) return NotFound();

            // Mark the book as available again
            borrow.Book.IsEnable = true;

            // Remove the borrow record from the database
            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync();

            // Redirect to the user's borrowed books page after returning the book
            return RedirectToAction("Borrow", new { id = borrow.UserId });
        }
    }
}
