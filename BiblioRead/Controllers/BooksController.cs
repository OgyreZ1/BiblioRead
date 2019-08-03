using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiblioRead.Controllers
{
    [Route("/api/books")]
    public class BooksController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper) {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks() {
            var books = await _context.Books.Include(b => b.Author).ToListAsync();

            

            var bookResources = new List<BookResource>();

            foreach (var book in books) {
                var bookRentals = _context.BookRentals.Where(br => br.BookId == book.Id);
                var rentalIds = new List<int>();

                foreach (var rentalLink in bookRentals)
                {
                    rentalIds.Add(rentalLink.RentalId);
                }

                var bookResource = _mapper.Map<Book, BookResource>(book);

                bookResources.Add(bookResource);
            }

            return Ok(bookResources);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id) {
            var bookInDb = await _context.Books.
                Include(b => b.Author)
                .Include(b => b.RentalLinks)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (bookInDb == null)
            {
                return NotFound();
            }

            var bookResource = _mapper.Map<Book, BookResource>(bookInDb);

            return Ok(bookResource);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> CreateBook([FromBody] BookResource bookResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (bookResource.Year <= 0 || bookResource.Year > DateTime.Now.Year) {
                ModelState.AddModelError("book", "The field Year must be between 0 and " 
                                                 + DateTime.Now.Year + ".");
                return BadRequest(ModelState);
            }

            var bookInDb = await _context.Books.Include(b => b.Author)
                .SingleOrDefaultAsync(b => b.Title == bookResource.Title 
                                           && b.Author.Name == bookResource.AuthorName 
                                           && b.Year == bookResource.Year);

            if (bookInDb != null) {
                ModelState.AddModelError("book", "This book is already exists.");
                return BadRequest(ModelState);
            }


            Author author = await _context.Authors.Include(a => a.Books)
                .SingleOrDefaultAsync(a => a.Name.Equals(bookResource.AuthorName));

            if (author == null)
            {
                author = new Author()
                {
                    Name = bookResource.AuthorName
                };
                await _context.Authors.AddAsync(author);
            }

            var book = new Book()
            {
                Author = author,
                Title = bookResource.Title,
                Year = bookResource.Year
            };

            author.Books.Add(book);

            book.AuthorId = author.Id;
            bookResource.AuthorId = book.AuthorId;
            
            await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();
            bookResource.Id = book.Id;
            return Ok(bookResource);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> DeleteBook(int id) {
            var bookInDb = await _context.Books.SingleOrDefaultAsync(b => b.Id == id);

            if (bookInDb == null) {
                return NotFound();
            }

            _context.Books.Remove(bookInDb);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> UpdateBook(int id,[Microsoft.AspNetCore.Mvc.FromBody] BookResource bookResource) {
            var bookInDb = await _context.Books.SingleOrDefaultAsync(b => b.Id == id);

            if (bookInDb == null) {
                return NotFound();
            }

            bookInDb.Title = bookResource.Title;
            bookInDb.Year = bookResource.Year;

            var authorInDb = await _context.Authors
                .SingleOrDefaultAsync(a => a.Id == bookInDb.AuthorId);

            if (authorInDb.Name.Equals(bookResource.AuthorName)) {
                bookResource.Id = bookInDb.Id;
                bookResource.AuthorId = bookInDb.AuthorId;
                await _context.SaveChangesAsync();
                return Ok(bookResource);
            }

            var author = new Author()
            {
                Name = bookResource.AuthorName
            };

            author.Books.Add(bookInDb);
            await _context.Authors.AddAsync(author);

            bookInDb.Author = author;
            bookInDb.AuthorId = author.Id;

            bookResource.AuthorId = bookInDb.AuthorId;
            bookResource.Id = bookInDb.Id;

            await _context.SaveChangesAsync();
            return Ok(bookResource);
        }

    }
}