﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BiblioRead.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ApplicationSettings _appSettings;
        

        public ApplicationUsersController(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings, ApplicationDbContext context) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        // api/ApplicationUsers/Register
        [HttpPost]
        [Route("Register")]
        public async Task<Object> PostApplicationUser(ApplicationUserResource user) {

            var applicationUser = new ApplicationUser() {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName
            };

            try {
                var result = await _userManager.CreateAsync(applicationUser, user.Password);
                await _userManager.AddToRoleAsync(applicationUser, user.Role);
                return Ok(result);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginResource loginResource) {
            var user = await _userManager.FindByNameAsync(loginResource.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginResource.Password)) {

                //Get role assigned to the user
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault()),
                    }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token });
            }
            else {
                return BadRequest(new { message = "Username or password is incorrect"});
            }
        }

        [HttpGet]
        [Route("{role}")]
        public async Task<IActionResult> GetUsersByRole(string role) {
            var users = await _userManager.GetUsersInRoleAsync(role);
            var userResources = new List<ApplicationUserResource>();

            foreach (var user in users) {
                var userRentals = _context.Rentals.Include(r => r.BooksLink)
                                                  .ThenInclude(bl => bl.Book)
                                                  .ThenInclude(b => b.Author)
                                                  .Where(r => r.User.Id == user.Id);

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

                var userResource = _mapper.Map<ApplicationUser, ApplicationUserResource>(user);
                userResource.Role = role;
                userResource.Rentals = rentalResources;

                userResources.Add(userResource);
            }

            return Ok(userResources);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(string id) {
            var userInDb = await _userManager.FindByIdAsync(id);

            if (userInDb == null)
                return NotFound();

            await _userManager.DeleteAsync(userInDb);
            return Ok();
        }

    }
}