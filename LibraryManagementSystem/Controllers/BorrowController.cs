using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowController : Controller
    {
        private readonly LibDbContext _context;
        public BorrowController(LibDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Borrow(int id)
        {
            var availableBooks = await _context.Books
                .Where(b => b.IsEnable)
                .ToListAsync();

            var borrowedBooks = await _context.Borrows
                .Where(b => b.UserId == id)
                .Include(b => b.Book)  // Kitap bilgisini de çekiyoruz
                .ToListAsync();

            var model = new BorrowViewModel
            {
                UserId = id,
                AvailableBooks = availableBooks,
                BorrowedBooks = borrowedBooks
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(int userId, int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null || !book.IsEnable)
            {
                return NotFound();
            }

            // Kitap ödünç alındı
            book.IsEnable = false;

            var borrow = new Borrow
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = DateTime.Now
            };

            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Return(int id)
        {
            var borrow = _context.Borrows
                                 .Include(b => b.Book) // Book ilişkisinin yüklendiğinden emin ol
                                 .FirstOrDefault(b => b.Id == id);

            if (borrow == null || borrow.Book == null)
            {
                return NotFound(); // Eğer kayıt bulunamazsa hata döndür
            }

            return View(borrow);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmReturn(int id)
        {
            var borrow = await _context.Borrows
        .Include(b => b.Book)
        .FirstOrDefaultAsync(b => b.Id == id);

            if (borrow == null)
            {
                return NotFound();
            }

            borrow.Book.IsEnable = true;

            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync();

            return RedirectToAction("Borrow", new { id = borrow.UserId });

        }
    }
}

