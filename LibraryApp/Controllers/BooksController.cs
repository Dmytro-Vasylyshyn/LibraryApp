using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models; 
using Microsoft.AspNetCore.Identity; 
using System.Threading.Tasks;
using System.Linq;

[Authorize] 
public class BooksController : Controller
{
    private readonly LibraryContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(LibraryContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Combine the two Index methods into one
    public async Task<IActionResult> Index(string searchTitle, string searchAuthor, string searchGenre)
    {
        var books = _context.Books.AsQueryable(); // Start with a queryable collection

        // Filter books based on search criteria
        if (!string.IsNullOrEmpty(searchTitle))
        {
            books = books.Where(b => b.Title.Contains(searchTitle));
        }

        if (!string.IsNullOrEmpty(searchAuthor))
        {
            books = books.Where(b => b.Author.Contains(searchAuthor));
        }

        if (!string.IsNullOrEmpty(searchGenre))
        {
            books = books.Where(b => b.Genre.Contains(searchGenre));
        }

        // Store search criteria in ViewData for the view to access
        ViewData["SearchTitle"] = searchTitle;
        ViewData["SearchAuthor"] = searchAuthor;
        ViewData["SearchGenre"] = searchGenre;

        return View(await books.ToListAsync()); // Return the filtered list
    }



    [HttpGet]
    public async Task<IActionResult> Search(string searchTitle, string searchAuthor, string searchGenre)
    {
        var books = _context.Books.AsQueryable();

        // Apply filters based on search criteria
        if (!string.IsNullOrEmpty(searchTitle))
        {
            books = books.Where(b => b.Title.Contains(searchTitle));
        }

        if (!string.IsNullOrEmpty(searchAuthor))
        {
            books = books.Where(b => b.Author.Contains(searchAuthor));
        }

        if (!string.IsNullOrEmpty(searchGenre))
        {
            books = books.Where(b => b.Genre.Contains(searchGenre));
        }

        var result = await books.ToListAsync();

        return PartialView("_BooksList", result); // Return a partial view with the filtered books
    }





    [Authorize(Roles = "Administrator")] 
    public IActionResult Create()
    {
        return View(new Book());
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Author,Genre,Year,CopiesAvailable")] Book book)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage); 
            }
            return View(book); 
        }

        _context.Add(book);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator")] 
    public async Task<IActionResult> Edit(int? id)
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
    [Authorize(Roles = "Administrator")] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,Year,CopiesAvailable")] Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound(); // Consider returning a view that informs the user
        }
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrator")] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }
}
