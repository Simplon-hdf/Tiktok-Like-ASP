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

        #region Profile
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

            return View(); //Show Profile.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Profile(EditProfileRequest profileRequest)
        {
            //In case session expired.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Home");
            }

            //Passwords and confirmed password must be the sames.
            if (profileRequest.Password != "" && profileRequest.Password != profileRequest.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("Profile");
            }

            //New username cannot be the same as the old one.
            string oldUsername = HttpContext.Session.GetString("Username");
            if (profileRequest.Username != "" && oldUsername == profileRequest.Username)
            {
                ModelState.AddModelError("", "Cannot update username: new is the same as previous.");
                return View("Profile");
            }

            //Check if another user has the new username.

            var userToUpdate = await _context.Persons.FirstOrDefaultAsync(user => user.Name == HttpContext.Session.GetString("Username"));

            if(userToUpdate == null)
            {
                ModelState.AddModelError("", "User does not exists ?");
                return View("Profile");
            } else
            {
                ModelState.AddModelError("", "User is: " + userToUpdate.Name);
            }

            string newPassword = userToUpdate.Password;
            if (profileRequest.Password != "") newPassword = profileRequest.Password;
            
            string newUsername = userToUpdate.Name;
            if(profileRequest.Username != "") newUsername = profileRequest.Username;

            userToUpdate.Name = newUsername;
            userToUpdate.Password = newPassword;

            /*if (await TryUpdateModelAsync(userToUpdate,
                "",
                user => user.Name,
                user => user.Password
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("Username", userToUpdate.Name);
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again or change informations.");
                    return View("Profile");
                }

                ModelState.AddModelError("", "The changes have been saved." + " New username:" + HttpContext.Session.GetString("Username"));
                return View("Profile");
            }
            else
            {
                return View("Profile");
            }*/

            /*_context.Update(userToUpdate);

            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("Username", userToUpdate.Name);
            ModelState.AddModelError("", "The changes have been saved." + " New username:" + HttpContext.Session.GetString("Username"));
            */return View("Profile");
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