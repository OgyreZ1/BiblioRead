using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiblioRead.Controllers
{
    [Route("/api/books")]
    public class BooksController : Controller {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks() {
            var books = await context.Books.Include(b => b.Author).ToListAsync();

            var booksRecources = mapper.Map<List<Book>, List<BookResource>>(books);
            return Ok(booksRecources);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id) {
            var bookInDb = await context.Books.Include(b => b.Author)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (bookInDb == null)
            {
                return NotFound();
            }

            var bookResource = mapper.Map<Book, BookResource>(bookInDb);

            return Ok(bookResource);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookResource bookResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (bookResource.Year <= 0 || bookResource.Year > DateTime.Now.Year) {
                ModelState.AddModelError("Year", "The field Year must be between 0 and " 
                                                 + DateTime.Now.Year + ".");
                return BadRequest(ModelState);
            }

            var bookInDb = await context.Books.Include(b => b.Author)
                .SingleOrDefaultAsync(b => b.Title == bookResource.Title);

            if (bookInDb != null && bookInDb.Author.Name == bookResource.AuthorName
                                 && bookInDb.Year == bookResource.Year) {
                ModelState.AddModelError("book", "This book is already exists.");
                return BadRequest(ModelState);
            }


            Author author = await context.Authors.Include(a => a.Books)
                .SingleOrDefaultAsync(a => a.Name.Equals(bookResource.AuthorName));

            if (author == null)
            {
                author = new Author()
                {
                    Name = bookResource.AuthorName
                };
                await context.Authors.AddAsync(author);
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
            
            await context.Books.AddAsync(book);

            await context.SaveChangesAsync();
            bookResource.Id = book.Id;
            return Ok(bookResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id) {
            var bookInDb = await context.Books.SingleOrDefaultAsync(b => b.Id == id);

            if (bookInDb == null) {
                return NotFound();
            }

            context.Books.Remove(bookInDb);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id,[FromBody] BookResource bookResource) {
            var bookInDb = await context.Books.SingleOrDefaultAsync(b => b.Id == id);

            if (bookInDb == null) {
                return NotFound();
            }

            bookInDb.Title = bookResource.Title;
            bookInDb.Year = bookResource.Year;

            var authorInDb = await context.Authors
                .SingleOrDefaultAsync(a => a.Id == bookInDb.AuthorId);

            if (authorInDb.Name.Equals(bookResource.AuthorName)) {
                bookResource.Id = bookInDb.Id;
                bookResource.AuthorId = bookInDb.AuthorId;
                await context.SaveChangesAsync();
                return Ok(bookResource);
            }

            var author = new Author()
            {
                Name = bookResource.AuthorName
            };

            author.Books.Add(bookInDb);
            await context.Authors.AddAsync(author);

            bookInDb.Author = author;
            bookInDb.AuthorId = author.Id;

            bookResource.AuthorId = bookInDb.AuthorId;
            bookResource.Id = bookInDb.Id;

            await context.SaveChangesAsync();
            return Ok(bookResource);
        }

    }
}