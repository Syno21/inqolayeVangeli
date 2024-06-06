using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InqolaYevangeli.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationDbContext _dbContext;

        public LoginController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Hash the entered password
                string hashedPassword;
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashedBytes.Length; i++)
                    {
                        builder.Append(hashedBytes[i].ToString("x2")); // Convert byte to hexadecimal string
                    }

                    hashedPassword = builder.ToString();
                }

                // Retrieve user from database based on username
                var user = _dbContext.Users.FirstOrDefault(u => u.Username == model.Username);

                if (user != null && user.Password == hashedPassword)
                {
                    // Create claims for authentication
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

                    // Conditionally include BranchID claim based on user's role
                    if (user.Role != "Super Admin")
                    {
                        claims.Add(new Claim("BranchID", user.BranchID.ToString()));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Other authentication properties if needed
                    };

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Redirect based on user's role
                    switch (user.Role)
                    {
                        case "Super Admin":
                            return RedirectToAction("Dashboard", "Dashboard");
                        case "Branch Manager":
                            return RedirectToAction("ManagerDashboard", "BranchManagerDashboard");
                        case "Regular User":
                            return RedirectToAction("Index", "GeneralUser");
                        default:
                            ModelState.AddModelError(string.Empty, "Unknown role");
                            break;
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
            }

            return View(model);
        }


    }
}
