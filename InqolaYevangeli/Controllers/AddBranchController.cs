using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Controllers
{
    public class AddBranchController : Controller
    {
        private ApplicationDbContext dbContext;
        public AddBranchController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult AddBranch()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBranch(AddBranchViewModel branchData)
        {
            if (!ModelState.IsValid)
            {
                // If model state is not valid, return the view with validation errors
                return View(branchData);
            }

            // Create a new branch entity and add it to the Branches table
            var newBranch = new Branch
            {
                BranchName = branchData.BranchName,
                Location = branchData.Location,
                Country = branchData.Country,
                State = branchData.State
            };

            await dbContext.Branches.AddAsync(newBranch);
            await dbContext.SaveChangesAsync();

            // If successfully added, redirect to a success page or another action
           // return RedirectToAction("AddMember");
            return RedirectToAction("AddBranch");
        }

        public IActionResult EditBranch(int id)
        {
            var branch = dbContext.Branches.FirstOrDefault(b => b.BranchID == id);

            if (branch == null)
            {
                return NotFound();
            }

            var branchViewModel = new AddBranchViewModel  // Assuming AddBranchViewModel is used for editing as well
            {
                BranchID = branch.BranchID,
                BranchName = branch.BranchName,
                Location = branch.Location,
                Country = branch.Country, // Retrieve country value from existing branch data
                State = branch.State
            };

            return View("AddBranch", branchViewModel); // Pass the AddBranchViewModel to the AddBranch view
        }
    }
}
