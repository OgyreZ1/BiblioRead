using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiblioRead.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiblioRead.Controllers
{
    public class BooksController : Controller {
        private readonly ApplicationDbContext context;

        public BooksController(ApplicationDbContext context) {
            this.context = context;
        }

        [HttpGet("/api/books")]
        public async Task<IEnumerable<Book>> GetBooks() {
            return await context.Books.Include(b => b.Author).ToListAsync();
        }
    }
}