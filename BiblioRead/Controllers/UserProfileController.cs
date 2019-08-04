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
    public class UserProfileController : ControllerBase {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserProfileController(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context) {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile() {
            string userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);

            var userResource = _mapper.Map<ApplicationUser, ApplicationUserResource>(user);

            var userRentals = _context.Rentals
            .Include(r => r.BooksLink)
            .ThenInclude(bl => bl.Book)
            .ThenInclude(b => b.Author)
            .Where(r => r.User.Id == userId).ToList();

            var rentalResources = new List<RentalResource>();
            foreach (var rental in userRentals)
            {
                var bookIdsInRental = rental.BooksLink.Select(bookLink => bookLink.BookId).ToList();

                var bookResources = rental.BooksLink.Select(bookRental => _mapper.Map<Book, BookResource>(bookRental.Book)).ToList();

                var rentalResource = _mapper.Map<Rental, RentalResource>(rental);
                rentalResource.BookIds = bookIdsInRental;
                rentalResource.Books = bookResources;

                rentalResources.Add(rentalResource);
            }

            userResource.Rentals = rentalResources;

            return Ok(userResource);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser(ApplicationUserResource resource) {

            var user = await _userManager.FindByIdAsync(resource.Id);
            if (user == null) return BadRequest();

            user.FullName = resource.FullName;
            user.UserName = resource.UserName;
            user.Email = resource.UserName;

            var userRole = _userManager.GetRolesAsync(user).Result;
            await _userManager.RemoveFromRolesAsync(user, userRole);
            await _userManager.AddToRoleAsync(user, resource.Role);

            return Ok();
        }


    }
}