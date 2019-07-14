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
    }
}