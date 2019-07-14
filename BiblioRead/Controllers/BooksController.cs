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
        public BookResource GetBook(int id) {
            var book = context.Books.Include(b => b.Author).SingleOrDefault(b => b.Id == id);

            return mapper.Map<Book, BookResource>(book);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] BookResource bookResource) {
            var bookInDb = context.Books.SingleOrDefault(b => b.Title == bookResource.Title);

            if (bookInDb != null && bookInDb.Author.Name == bookResource.AuthorName && bookInDb.Year == bookResource.Year) {
                return BadRequest();
            }


            Author author = context.Authors.SingleOrDefault(a => a.Name.Equals(bookResource.AuthorName));
            if (author == null)
            {
                author = new Author()
                {
                    Name = bookResource.AuthorName
                };
            }

            var book = new Book()
            {
                Author = author,
                AuthorId = author.Id,
                Title = bookResource.Title,
                Year = bookResource.Year
            };

            author.Books.Add(book);
            context.Books.Add(book);

            context.SaveChanges();
            return Ok(bookResource);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id) {
            var book = context.Books.SingleOrDefault(b => b.Id == id);

            if (book == null)
                return BadRequest();

            context.Books.Remove(book);
            context.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, BookResource bookResource) {
            /*var bookInDb = context.Books.SingleOrDefault(b => b.Id == id);
            if (bookInDb == null)
                return BadRequest();

            bookInDb.Title = bookResource.Title;
            bookInDb.Year = bookInDb.Year;

            if (bookInDb.AuthorId == bookResource.Author.Id) {
                context.SaveChanges();
                return Ok();
            }

            Author author = context.Authors.SingleOrDefault(a => a.Name == bookResource.Author.Name);
            if (author == null) {
                author = new Author() {
                    Name = bookResource.Author.Name
                };
            }
            author.Books.Add(bookInDb);

            context.SaveChanges();*/

            return Ok();
        }

    }
}