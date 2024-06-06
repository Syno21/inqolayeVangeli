using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Controllers
{
    

    public class AddUserController : Controller
    {

        private ApplicationDbContext _dbContext;

        public AddUserController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult AddUser()
        {
            var branches = _dbContext.Branches.ToList();
            ViewBag.Branches = branches;
            var model = new AddUserViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddUser(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {

                // Check if the BranchID exists in the Branches table
                var branchExists = _dbContext.Branches.Any(b => b.BranchID == model.BranchID);
                //if (!branchExists)
                //{
               //     ModelState.AddModelError("BranchID", "The specified branch ID does not exist.");
                //    return View(model);
               // }
                // Hash the password
                string hashedPassword = HashPassword(model.Password);
            
                // Create a new User instance
                var newUser = new User
                {
                    Username = model.Username,
                    Password = hashedPassword,
                    Role = model.Role,
                    BranchID = model.Role == "Super Admin" ? 2 : model.BranchID
                };

                // Add the new user to the database
                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                return RedirectToAction("AddUser", "AddUser"); // Redirect to the home page or any other page
            }

            // If model state is not valid, return the view with validation errors
            return View(model);
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2")); // Convert byte to hexadecimal string
                }

                return builder.ToString();
            }


        }



      

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Assuming you need to populate dropdowns for edit, similar to AddMember action
            var branches = await _dbContext.Branches.ToListAsync();
            ViewBag.Branches = branches;

            var membershipStatuses = await _dbContext.MembershipStatuses.ToListAsync();
            ViewBag.MembershipStatuses = membershipStatuses;

            // Find the latest status change record for the given member ID
            var latestStatusChange = await _dbContext.MemberStatusHistory
                .Where(msh => msh.MemberID == id)
                .OrderByDescending(msh => msh.DateChanged)
                .FirstOrDefaultAsync();

            // Assuming you have a mapping from your Member entity to your AddMemberViewModel
            var viewModel = new AddUserViewModel
            {
                Username = user.Username,
                Password = "",
                Role = user.Role,

                BranchID = user.BranchID,


            };

            return View("AddUser", viewModel);

        }
    }
}
