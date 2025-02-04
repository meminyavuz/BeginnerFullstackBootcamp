using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly LibDbContext _context;

        public UserController(LibDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.Users.ToListAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "User");
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id != 0)
            {
                var selectedUser = await _context.Users.FindAsync(id);
                return View(selectedUser);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(User updatedUser, int id)
        {
            if (ModelState.IsValid)
            {
                _context.Update(updatedUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "User");
            }
            return View(updatedUser);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index"); // Silme işleminden sonra liste sayfasına yönlendirilir
        }
    }
}