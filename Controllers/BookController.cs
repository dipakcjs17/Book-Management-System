using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bookmanagement.Models;
using bookmanagement.ViewModels;



namespace bookmanagement.Controllers;
public class BookController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static List<Book> _books = new List<Book>(); //This will hold the book collection

    public BookController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

     // Display a list of all books in the system
    public IActionResult Index(string searchTerm, int page = 1)
    {
        //Abiral Tamang 23375/076
        const int pageSize = 5;

        // Apply search functionality
        var filteredBooks = string.IsNullOrWhiteSpace(searchTerm)
            ? _books
            : _books.Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                b.Genre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();

        // Apply pagination
        var totalCount = filteredBooks.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var booksToDisplay = filteredBooks
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Pass data to the view
        var model = new BookIndexViewModel
        {
            Books = booksToDisplay,
            SearchTerm = searchTerm,
            Pagination = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages
            }
        };
        return View(model);
    }

    // Add a new book to the system
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            // Generate a unique ID for the book (assuming no database)
            book.Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;

            _books.Add(book);

            return RedirectToAction("Index");
        }

        return View(book);
    }

     // Edit existing book information
    public IActionResult Edit(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

   [HttpPost]
    public IActionResult Edit(Book updatedBook)
    {
        if (ModelState.IsValid)
        {
            var book = _books.FirstOrDefault(b => b.Id == updatedBook.Id);

            if (book == null)
            {
                return NotFound();
            }

            // Update the book's information
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.PublicationDate = updatedBook.PublicationDate;
            book.Genre = updatedBook.Genre;

            return RedirectToAction("Index");
        }

        return View(updatedBook);
    }

    // Delete a book from the system
    public IActionResult Delete(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        _books.Remove(book);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
