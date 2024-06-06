using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Controllers
{
    public class ViewUserController : Controller
    {


        private ApplicationDbContext _dbContext;

        public ViewUserController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
      


      
        public async Task<IActionResult> ViewUser(int? branchId)
        {


            var members = await _dbContext.Users.ToListAsync();

            ViewBag.Branches = await _dbContext.Branches.ToListAsync();

            IQueryable<User> usersQuery = _dbContext.Users.Include(u => u.Branch);

            if (branchId.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.BranchID == branchId.Value);
            }

            var users = await usersQuery.ToListAsync();
            ViewBag.Branches = await _dbContext.Branches.ToListAsync();

            return View(users);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            // Redirect to the ViewDelete action after deleting the member
            return RedirectToAction(nameof(ViewUser));
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
                Password ="",
                Role = user.Role,
                
                BranchID = user.BranchID,
               
               
            };

            return View("AddUser", viewModel);
           
        }


    }
}
