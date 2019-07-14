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
        public async Task<IEnumerable<BookResource>> GetBooks() {
            var books = await context.Books.Include(b => b.Author).ToListAsync();

            return mapper.Map<List<Book>, List<BookResource>>(books);
        }

        [HttpGet("{id}")]
        public async Task<BookResource> GetBook(int id) {
            var book = await context.Books.Include(b => b.Author).SingleOrDefaultAsync(b => b.Id == id);

            return mapper.Map<Book, BookResource>(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookResource bookResource) {
            var bookInDb = context.Books.SingleOrDefault(b => b.Title == bookResource.Title);

            if (bookInDb != null && bookInDb.Author.Name == bookResource.AuthorName && bookInDb.Year == bookResource.Year) {
                return BadRequest();
            }


            Author author = await context.Authors.SingleOrDefaultAsync(a => a.Name.Equals(bookResource.AuthorName));
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
            var book = await context.Books.SingleOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return BadRequest();

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id,[FromBody] BookResource bookResource) {
            var bookInDb = await context.Books.SingleOrDefaultAsync(b => b.Id == id);

            if (bookInDb == null)
                return BadRequest();

            bookInDb.Title = bookResource.Title;
            bookInDb.Year = bookResource.Year;

            var authorInDb = await context.Authors.SingleOrDefaultAsync(a => a.Id == bookInDb.AuthorId);
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