using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Controllers
{
    public class BranchController : Controller
    {
       

        private readonly ApplicationDbContext _dbContext;

        public BranchController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageBranches()
        {
            var branches = _dbContext.Branches.ToList();

            var branchViewModels = branches.Select(branch => new BranchViewModel
            {
                BranchID = branch.BranchID,
                BranchName = branch.BranchName,
                Location = branch.Location,
                Country = branch.Country,
                State = branch.State,
            }).ToList();

            return View(branchViewModels);
        }

        public IActionResult EditBranch(int id)
        {
            var branch = _dbContext.Branches.FirstOrDefault(b => b.BranchID == id);

            if (branch == null)
            {
                return NotFound();
            }

            var branchViewModel = new AddBranchViewModel  // Assuming AddBranchViewModel is used for editing as well
            {
                BranchID = branch.BranchID,
                BranchName = branch.BranchName,
                Location = branch.Location,
                 Country = branch.Country,
                State = branch.State,
            };

            return View("AddBranch", branchViewModel); // Pass the AddBranchViewModel to the AddBranch view
        }




        public IActionResult DeleteBranch(int id)
        {
            var branch = _dbContext.Branches.FirstOrDefault(b => b.BranchID == id);



            // Check if there are any associated users
            var associatedUsers = _dbContext.Users.Where(u => u.BranchID == id).ToList();

            if (associatedUsers.Any())
            {
                // You can handle the presence of associated users here
                // For example, you can display a message to the user or prompt them to reassign users before deleting the branch
                return BadRequest("Cannot delete branch because it has associated users.");
            }
            if (branch != null)
            {
                _dbContext.Branches.Remove(branch);
                _dbContext.SaveChanges();
            }

            return RedirectToAction(nameof(AddBranch));
        }

        public IActionResult AddBranch()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBranch(BranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var branch = new Branch
                {
                    BranchName = model.BranchName,
                    Location = model.Location,
                    Country = model.Country,
                    State = model.State,
                };

                _dbContext.Branches.Add(branch);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(ManageBranches));
            }

            return View(model);
        }
    }
}
