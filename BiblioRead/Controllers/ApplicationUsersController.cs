using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiblioRead.Controllers.Resources;
using BiblioRead.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BiblioRead.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public ApplicationUsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
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
                return Ok(result);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        
    }
}