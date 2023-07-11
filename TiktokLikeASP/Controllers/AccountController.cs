using Microsoft.AspNetCore.Mvc;
using TiktokLikeASP.Context;
using TiktokLikeASP.Models.ViewModels;
using TiktokLikeASP.Models;
using Microsoft.AspNetCore.Identity;

namespace TiktokLikeASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Store Database context on application start.
        /// </summary>
        /// <param name="appDbContext">Database context of the application</param>
        public AccountController(ApplicationDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // Profile page
        public IActionResult Index()
        {
            return View();
        }

        #region REGISTER
        [HttpGet]
        public IActionResult Register() 
        { 
            return View(); //Show Register.cshtml 
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest registerRequest) 
        {
            var duplicateUsernameDbEntry = _context.Users.FirstOrDefault(
                acc => acc.Name == registerRequest.Username
            );
            var duplicateEmailDbEntry = _context.Users.FirstOrDefault(
                acc => acc.Email == registerRequest.Email
            );

            // If duplicate email or username founded, do not save new user.
            if(duplicateUsernameDbEntry != null || duplicateEmailDbEntry != null)
            {
                ModelState.AddModelError("", "Email or Username already exists.");
                return View("Register"); //If registering fail, reload the HttpGet Register()
            }

            var newUser = new User
            {
                Name = registerRequest.Username,
                Email = registerRequest.Email,
                Password = PasswordHashing(registerRequest.Password)
            };

            // Hash and Salt that password !
            _context.Users.Add(newUser);

            // If register succed:
            return RedirectToAction("Login");
        }

        private string PasswordHashing(string password)
        {
            /*
             * Implement hashing and salting.
             * // (e.g., using bcrypt, PBKDF2, or another secure algorithm)
             */

            return password;
        }
        #endregion

        #region LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View(); //Show Login.cshtml
        }

        [HttpPost]
        public IActionResult Login(LoginRequest loginRequest)
        {
            string hashedPassword = PasswordHashing(loginRequest.Password);

            //To-do: Must be protected from SQL Injection
            var searchUserDbEntry = _context.Users.FirstOrDefault(
                acc => acc.Name == loginRequest.Username
            );

            // Cannot find the given username in database.
            if(searchUserDbEntry == null)
            {
                ModelState.AddModelError("", "Cannot find account for given username and password");
                return View("Login"); //Return to Login.cshtml with an error to show.
            }

            /* 
             * 
             * Create a cookie to allow the user to stay signed in
             * 
             */

            return View("Login"); //Show Login.cshtml
        }
        #endregion
    }
}
