using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using BCrypt.Net; //// Added in 6.1
using Microsoft.AspNetCore.Identity; // Added in 6.1

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserDataAccess _userDataAccess;
        private readonly IPasswordHasher<UserModel> _passwordHasher;

        public UsersController(
            IUserDataAccess userDataAccess,
            IPasswordHasher<UserModel> passwordHasher
        )
        {
            _userDataAccess = userDataAccess;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        //** for testing only:
        [AllowAnonymous]
        public ActionResult<IEnumerable<UserModel>> GetAllUsers()
        {
            return Ok(_userDataAccess.GetAllUsers());
        }

        [HttpGet("admin")]
        [Authorize(Policy = "AdminOnly")]
        public ActionResult<IEnumerable<UserModel>> GetAdminUsers()
        {
            return Ok(_userDataAccess.GetUsersByRole("Admin"));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserOnly")]
        public ActionResult<UserModel> GetUserById(int id)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        //** Used [AllowAnonymous]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<UserModel> CreateUser(UserModel newUser)
        {
            var existingUser = _userDataAccess.GetUserByEmail(newUser.Email);
            if (existingUser != null)
                return Conflict(new { message = "Email already exists." });

            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);
            newUser.CreatedDate = DateTime.Now;
            newUser.ModifiedDate = DateTime.Now;
            _userDataAccess.AddUser(newUser);
            
            var createdUser = _userDataAccess.GetUserByEmail(newUser.Email);
            if (createdUser == null)
                return Problem("Failed to retrieve the newly created user.");

            return _userDataAccess.GetUserByEmail(newUser.Email);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUser(int id, UserModel updatedUser)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
                return NotFound();
                
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Role = updatedUser.Role;
            user.Description = updatedUser.Description;
            user.ModifiedDate = DateTime.Now;

            _userDataAccess.UpdateUser(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
                return NotFound();

            _userDataAccess.DeleteUser(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUserPassword(int id, LoginModel loginModel)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
                return NotFound();

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Password);
            user.ModifiedDate = DateTime.Now;

            _userDataAccess.UpdateUser(user);
            return NoContent();
        }
    }
}
