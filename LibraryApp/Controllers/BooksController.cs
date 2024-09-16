using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models; 
using System.Threading.Tasks;
using System.Linq;

public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchTitle, string searchAuthor, string searchGenre)
    {
        var books = from b in _context.Books
                    select b;

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

        return View(await books.ToListAsync());
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Author,Genre,Year,Copies")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,Year,Copies")] Book book)
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

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
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
