using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using InqolaYevangeli.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Controllers
{
    public class ViewDeleteController : Controller
    {
        private ApplicationDbContext _dbContext;

        public ViewDeleteController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }




        public async Task<IActionResult> ViewDelete(int? branchId)
        {
            IQueryable<Member> membersQuery = _dbContext.Members.Include(m => m.Branch);

            if (branchId.HasValue)
            {
                membersQuery = membersQuery.Where(m => m.BranchID == branchId.Value);
            }

            var members = await membersQuery.ToListAsync();

            ViewBag.Branches = await _dbContext.Branches.ToListAsync();

            return View(members);
        }

        public IActionResult AddMember()
        {
            return View();
        }



        public async Task<IActionResult> Delete(int id)
        {
            var member = await _dbContext.Members.FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            _dbContext.Members.Remove(member);
            await _dbContext.SaveChangesAsync();

            // Redirect to the ViewDelete action after deleting the member
            return RedirectToAction(nameof(ViewDelete));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _dbContext.Members.FindAsync(id);
            if (member == null)
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
            var viewModel = new AddMemberViewModel
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                DateOfBirth = member.DateOfBirth,
                Gender = member.Gender,
                BranchID = member.BranchID,
                // Assign StatusID here based on member's history or other source
                StatusID = latestStatusChange != null ? latestStatusChange.StatusID : 0 // If no status change record found, set a default value
            };

            return View("AddMember", viewModel);
        }





    }
}
