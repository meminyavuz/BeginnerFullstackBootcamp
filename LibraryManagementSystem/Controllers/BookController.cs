using System.Reflection.Metadata.Ecma335;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly LibDbContext _context;

        public BookController(LibDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.Books.ToListAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Book");
            }
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id != 0)
            {
                var selectedBook = await _context.Books.FindAsync(id);
                return View(selectedBook);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(Book updatedBook, int id)
        {
            if (ModelState.IsValid)
            {
                _context.Update(updatedBook);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Book");
            }
            return View(updatedBook);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _context.Books.Remove(book); 
                await _context.SaveChangesAsync(); 
            }
            return RedirectToAction(nameof(Index)); // Silme işleminden sonra liste sayfasına yönlendirilir
        }
    }
}