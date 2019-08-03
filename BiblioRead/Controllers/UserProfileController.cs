using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BiblioRead.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserProfileController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile() {
            string userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);

            return new {
                user.Id,
                user.FullName,
                user.Email,
                user.UserName
            };
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

        /*[HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("ForAdmin")]
        public string GetForAdmin() { 
            return "Web method for Admin";
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("ForCustomer")]
        public string GetForCustomer() {
            return "Web method for Customer";
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Librarian")]
        [Route("ForAdminOrLibrarian")]
        public string GetForAdminOrLibrarian() {
            return "Web method for Admin or Librarian";
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Customer, Librarian")]
        [Route("ForAdminOrLibrarianOrCustomer")]
        public string GetForAdminOrLibrarianOrCustomer() {
            return "Web method for Admin, Librarian or Customer";
        }*/

    }
}