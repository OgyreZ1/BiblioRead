using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiblioRead.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RentalsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper) {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRentals() {
            var rentals = await _context.Rentals.Include(r => r.BooksLink).Include(r => r.User).ToListAsync();

            var rentalResources = new List<RentalResource>();

            /*List<int> bookIds = new List<int>();

            foreach (var rental in rentals) {
                
            }*/
            foreach (var rental in rentals) {
                List<int> bookIdsInRental = new List<int>();
                foreach (var bookLink in rental.BooksLink) {
                    bookIdsInRental.Add(bookLink.BookId);
                }

                RentalResource resource = new RentalResource() {
                    Id = rental.Id,
                    UserId = rental.User.Id,
                    BookIds = bookIdsInRental,
                    DateCreated = rental.DateCreated,
                    EndingDate = rental.EndingDate,
                    IsCompleted = rental.IsCompleted
                };

                rentalResources.Add(resource);

            }

            return Ok(rentalResources);
        }

        [HttpPost]
        public async Task<IActionResult> PostRental(RentalResource resource) {
            var rental = new Rental() {
                User = await _userManager.FindByIdAsync(resource.UserId),
                DateCreated = DateTime.Now,
                EndingDate = DateTime.Now.AddDays(1),
                IsCompleted = false
            };

            var booksLink = new List<BookRental>();

            foreach (var bookId in resource.BookIds)
            {
                var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.RentalLinks)
                .SingleOrDefaultAsync(b => b.Id == bookId);

                BookRental bookRental = new BookRental() {
                    Book = book,
                    BookId = book.Id,
                    Rental = rental,
                    RentalId = rental.Id
                };

                if (book != null) {
                    booksLink.Add(bookRental);
                }

                book.RentalLinks.Add(bookRental);
            }

            rental.BooksLink = booksLink;

            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();

            return Ok(resource);
        }
    }
}