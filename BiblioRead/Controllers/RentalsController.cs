using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetRentals() {
            var rentals = await _context.Rentals
            .Include(r => r.BooksLink)
            .ThenInclude(bl => bl.Book)
            .Include(r => r.User)
            .ToListAsync();

            var rentalResources = new List<RentalResource>();
            foreach (var rental in rentals) {
                var bookIdsInRental = rental.BooksLink.Select(bookLink => bookLink.BookId).ToList();

                var bookResources = rental.BooksLink.Select(bookRental => _mapper.Map<Book, BookResource>(bookRental.Book)).ToList();

                var rentalResource = _mapper.Map<Rental, RentalResource>(rental);
                rentalResource.BookIds = bookIdsInRental;
                rentalResource.Books = bookResources;

                rentalResources.Add(rentalResource);
            }

            return Ok(rentalResources);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetRental(int id) {
            var rental = await _context.Rentals
            .Include(r => r.BooksLink)
            .ThenInclude(bl => bl.Book)
            .Include(r => r.User)
            .SingleOrDefaultAsync(r => r.Id == id);

            var bookResources = new List<BookResource>();
            foreach (var bookRental in rental.BooksLink) {
                bookResources.Add(_mapper.Map<Book, BookResource>(bookRental.Book));
            }
            var bookIdsInRental = rental.BooksLink.Select(bookLink => bookLink.BookId).ToList();

            var rentalResource = _mapper.Map<Rental, RentalResource>(rental);
            rentalResource.BookIds = bookIdsInRental;
            rentalResource.Books = bookResources;

            return Ok(rentalResource);
        }

        [HttpGet]
        [Route("user/{userName}")]
        [Authorize]
        public async Task<IActionResult> GetRentalsByUserName(string userName) {
            var user = await _userManager.FindByNameAsync(userName);

            var rentals = _context.Rentals
            .Include(r => r.BooksLink)
            .ThenInclude(bl => bl.Book)
            .Include(r => r.User)
            .Where(r => r.User.Id == user.Id).ToList();

            var rentalResources = new List<RentalResource>();
            foreach (var rental in rentals)
            {
                var bookIdsInRental = rental.BooksLink.Select(bookLink => bookLink.BookId).ToList();

                var bookResources = rental.BooksLink.Select(bookRental => _mapper.Map<Book, BookResource>(bookRental.Book)).ToList();

                var rentalResource = _mapper.Map<Rental, RentalResource>(rental);
                rentalResource.BookIds = bookIdsInRental;
                rentalResource.Books = bookResources;

                rentalResources.Add(rentalResource);
            }

            return Ok(rentalResources);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostRental(RentalResource resource) {
            var user = await _userManager.FindByIdAsync(resource.UserId);

            var rental = new Rental() {
                User = user,
                DateCreated = DateTime.Now,
                DeadlineDate = DateTime.Now.AddDays(1),
                IsFinished = false
            };

            var booksLink = new List<BookRental>();

            foreach (var bookId in resource.BookIds)
            {
                var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.RentalLinks)
                .SingleOrDefaultAsync(b => b.Id == bookId);

                var bookRental = new BookRental() {
                    Book = book,
                    BookId = book.Id,
                    Rental = rental,
                    RentalId = rental.Id
                };

                booksLink.Add(bookRental);

                book.RentalLinks.Add(bookRental);
            }

            rental.BooksLink = booksLink;

            user.Rentals.Add(rental);

            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();

            return Ok(resource);
        }

        [HttpPut]
        [Route("finish/{id}")]
        public async Task<IActionResult> FinishRental(int id) {
            var rental = await _context.Rentals.SingleOrDefaultAsync(r => r.Id == id);
            rental.IsFinished = true;
            rental.DateFinished = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}