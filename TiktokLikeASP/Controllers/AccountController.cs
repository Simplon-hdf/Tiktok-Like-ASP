using Microsoft.AspNetCore.Mvc;
using TiktokLikeASP.Context;
using TiktokLikeASP.Models.ViewModels;
using TiktokLikeASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace TiktokLikeASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static bool changeMades = false;
        private static string changesResume = "";

        /// <summary>
        /// Store Database context on application start.
        /// </summary>
        /// <param name="appDbContext">Database context of the application</param>
        public AccountController(ApplicationDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }




        
        [HttpGet]
        public IActionResult Profile()
        {
            //If someone try to connect to its profile without being logged in.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                ViewData["Username"] = HttpContext.Session.GetString("Username");
            }

            if (changeMades)
            {
                ViewData["ChangesMade"] = changesResume;
                changeMades = false;
            } 
            else
            {
                ViewData["ChangesMade"] = "";
            }

            return View(); //Show Profile.cshtml
        }




        #region Edit Username and Password
        
        [HttpPost]
        public ActionResult RedirectToEditUsername()
        {
            return View("EditUsername");
        }

        [HttpGet]
        public IActionResult EditUsername()
        {
            //If someone try to connect to its profile without being logged in.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Username"] = HttpContext.Session.GetString("Username");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUsername(string username)
        {
            //In case session expired.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Home");
            }

            var userToUpdate = await _context.Persons.FirstOrDefaultAsync(user => user.Name == HttpContext.Session.GetString("Username"));

            #region Search errors
            if (userToUpdate == null)
            {
                ModelState.AddModelError("", "User does not exists ?");
                return View("EditUsername");
            }

            //New username cannot be the same as the old one.
            string oldUsername = HttpContext.Session.GetString("Username");
            if (username != "" && oldUsername == username)
            {
                ModelState.AddModelError("", "Cannot update username: new is the same as previous.");
                return View("EditUsername");
            }

            //If another user has the new username, current user cannot take it.
            var newUsernameTaken = await _context.Persons.FirstOrDefaultAsync(
                user => user.Name == username
            );

            if (newUsernameTaken != null)
            {
                ModelState.AddModelError("", "New username isn't available.");
                return View("EditUsername");
            }
            #endregion

            string newUsername = userToUpdate.Name;
            if(username != "" && username != null)
                newUsername = username;
            
            userToUpdate.Name = newUsername;
            _context.Attach(userToUpdate).Property(u => u.Name).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Error when processing the request. Please try again");
                throw;
            }

            HttpContext.Session.SetString("Username", userToUpdate.Name);
            changeMades = true;
            changesResume =
                "The changes have been saved. \n" +
                " New username: " +
                HttpContext.Session.GetString("Username");
            
            return Redirect("Profile");
        }

        [HttpPost]
        public ActionResult RedirectToEditPassword()
        {
            return View("EditPassword");
        }

        [HttpGet]
        public IActionResult EditPassword()
        {
            //If someone try to connect to its profile without being logged in.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Username"] = HttpContext.Session.GetString("Username");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(string currentpassword, string newpassword, string confirmNewPassword)
        {
            //In case the session expired.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Home");
            }

            var userToUpdate = await _context.Persons.FirstOrDefaultAsync(user => user.Name == HttpContext.Session.GetString("Username"));

            #region Search errors
            if (userToUpdate == null)
            {
                ModelState.AddModelError("", "User does not exists ?");
                return View("EditPassword");
            }

            // Password isn't correct
            if (PasswordHashing(currentpassword) != userToUpdate.Password)
            {
                ModelState.AddModelError("", "Incorrect password.");
                return View("EditPassword");
            }

            //Passwords and confirmed password must be the sames.
            if (newpassword != "" && newpassword != confirmNewPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("EditPassword");
            }

            // Cannot update password if it's the same as the old one
            if(PasswordHashing(newpassword) == userToUpdate.Password)
            {
                ModelState.AddModelError("", "The new password cannot be the same as the current one");
                return View("EditPassword");
            }
            #endregion

            string updatedPassword = userToUpdate.Password;
            if (newpassword != "" && newpassword != null)
                updatedPassword = PasswordHashing(newpassword);

            userToUpdate.Password = updatedPassword;
            _context.Attach(userToUpdate).Property(u => u.Password).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Error when processing the request. Please try again");
                throw;
            }
            
            changeMades = true;
            changesResume = "The changes have been saved.";

            return Redirect("Profile");
        }
        #endregion



        #region REGISTER
        [HttpGet]
        public IActionResult Register() 
        { 
            return View(); //Show Register.cshtml 
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest registerRequest) 
        {
            if(registerRequest.Password != registerRequest.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords don't match.");
                return View("Register"); //If registering fail, reload the HttpGet Register()
            }

            var duplicateUsernameDbEntry = _context.Persons.FirstOrDefault(
                acc => acc.Name == registerRequest.Username
            );
            var duplicateEmailDbEntry = _context.Persons.FirstOrDefault(
                acc => acc.Email == registerRequest.Email
            );

            // If duplicate email or username founded, do not save new user.
            if(duplicateUsernameDbEntry != null || duplicateEmailDbEntry != null)
            {
                ModelState.AddModelError("", "Email or Username already exists.");
                return View("Register"); //If registering fail, reload the HttpGet Register()
            }

            var newUser = new Person
            {
                Name = registerRequest.Username,
                Email = registerRequest.Email,
                Password = PasswordHashing(registerRequest.Password)
            };

            // Hash and Salt that password !
            _context.Persons.Add(newUser);
            _context.SaveChanges();

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

        private string DecrytPassword(string hashedPssword)
        {
            /* Is that even possible ? */

            return hashedPssword;
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
            var searchUserDbEntry = _context.Persons.FirstOrDefault(
                acc => acc.Name == loginRequest.Username
            );

            // Cannot find the given username in database and passwords don't match.
            if(searchUserDbEntry == null || searchUserDbEntry.Password != hashedPassword)
            {
                ModelState.AddModelError("", "Cannot find account for given username and password");
                return View("Login"); //Return to Login.cshtml with an error to show.
            }

            /* 
             * Create a session to allow the user to stay signed in. Until he close its browser.
             */
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                // Set the session value
                HttpContext.Session.SetString("UserId", searchUserDbEntry.Id.ToString());
                HttpContext.Session.SetString("Username", searchUserDbEntry.Name);
            }
            //ModelState.AddModelError("", "Perfect"); //That's not how you should use it!
            return RedirectToAction("Index", "Home"); //Should later redirect to the feed of posts.
        }
        #endregion



        #region LOGOUT
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}