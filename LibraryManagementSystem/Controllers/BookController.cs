using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly LibDbContext _context;

        // Constructor to initialize the database context
        public BookController(LibDbContext context)
        {
            _context = context;
        }

        // Action to display the list of books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // Action to display the Create view
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Action to handle the POST request for creating a new book
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);  // Add the book to the context
                await _context.SaveChangesAsync();  // Save changes to the database
                return RedirectToAction("Index", "Book");  // Redirect to the list page
            }
            return View(book);  // Return the same view if the model is not valid
        }

        // Action to display the Update view for a specific book
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();  // Return NotFound if no id is provided

            var selectedBook = await _context.Books.FindAsync(id);  // Find the book by id

            if (selectedBook == null) return NotFound();  // Return NotFound if the book doesn't exist

            // If the book is currently borrowed, show an error message and redirect
            if (!selectedBook.IsEnable)
            {
                TempData["ErrorMessage"] = "This book is currently borrowed and cannot be updated.";
                return RedirectToAction("Index");
            }

            return View(selectedBook);  // Return the book for editing
        }

        // Action to handle the POST request for updating a book's details
        [HttpPost]
        public async Task<IActionResult> Update(Book updatedBook, int id)
        {
            if (ModelState.IsValid)
            {
                _context.Update(updatedBook);  // Update the book details in the context
                await _context.SaveChangesAsync();  // Save changes to the database
                return RedirectToAction("Index", "Book");  // Redirect to the list page
            }
            return View(updatedBook);  // Return the same view if the model is not valid
        }

        // Action to display the Delete view for a specific book
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();  // Return NotFound if no id is provided
            
            var selectedBook = await _context.Books.FindAsync(id);  // Find the book by id

            if (selectedBook == null) return NotFound();  // Return NotFound if the book doesn't exist

            // If the book is currently borrowed, show an error message and redirect
            if (!selectedBook.IsEnable)
            {
                TempData["ErrorMessage"] = "This book is currently borrowed and cannot be deleted.";
                return RedirectToAction("Index");
            }
            
            return View(selectedBook);  // Return the book for deletion confirmation
        }

        // Action to handle the POST request for deleting a book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);  // Find the book by id
            if (book != null)
            {
                _context.Books.Remove(book);  // Remove the book from the context
                await _context.SaveChangesAsync();  // Save changes to the database
            }
            return RedirectToAction("Index");  // Redirect to the list page after deletion
        }
    }
}
