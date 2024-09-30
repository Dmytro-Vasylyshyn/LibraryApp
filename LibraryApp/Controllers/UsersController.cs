using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Models;
using LibraryApp.Data;
using LibraryApp.Helpers;

namespace LibraryApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly LibraryContext _context;

        public UsersController(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string searchBy, int pageNumber = 1, int pageSize = 10)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                usersQuery = searchBy switch
                {
                    "FirstName" => usersQuery.Where(u => u.FirstName.Contains(searchString)),
                    "LastName" => usersQuery.Where(u => u.LastName.Contains(searchString)),
                    "Email" => usersQuery.Where(u => u.Email.Contains(searchString)),
                    _ => usersQuery
                };
            }

            var paginatedUsers = await PaginatedList<ApplicationUser>.CreateAsync(usersQuery.AsNoTracking(), pageNumber, pageSize);
            return View(paginatedUsers);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
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
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.Email; // Set the username to be the same as the email
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
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

        // Action for displaying the list of books borrowed by a user
        public async Task<IActionResult> UserBorrowedBooks(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var borrowedBooks = await _context.Borrowing
                .Include(bb => bb.Book)
                .Where(bb => bb.UserId == id && bb.ReturnedDate == null)
                .ToListAsync();

            if (!borrowedBooks.Any())
            {
                ViewBag.Message = "No books currently borrowed.";
            }

            return View(borrowedBooks);
        }

        [Authorize(Roles = "User, Administrator")]
        public async Task<IActionResult> Borrow(string userId, int bookId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            var book = await _context.Books.FindAsync(bookId);

            if (!userExists || book == null)
            {
                ViewBag.ErrorMessage = "User or Book does not exist.";
                return View("Error");
            }

            if (book.CopiesAvailable <= 0)
            {
                ViewBag.ErrorMessage = "No copies of this book are available.";
                return RedirectToAction("Index");
            }

            var borrowing = new Borrowing
            {
                UserId = userId,
                BookId = bookId,
                BorrowedDate = DateTime.UtcNow
            };

            book.CopiesAvailable -= 1;
            _context.Borrowing.Add(borrowing);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserBorrowedBooks", new { id = userId });
        }

        public async Task<IActionResult> Return(int id)
        {
            var borrowing = await _context.Borrowing.FindAsync(id);
            if (borrowing != null && borrowing.ReturnedDate == null)
            {
                borrowing.ReturnedDate = DateTime.UtcNow;

                var book = await _context.Books.FindAsync(borrowing.BookId);
                if (book != null)
                {
                    book.CopiesAvailable += 1;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserBorrowedBooks", new { id = borrowing?.UserId });
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
