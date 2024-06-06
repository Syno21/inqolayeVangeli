using InqolaYevangeli.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InqolaYevangeli.Controllers
{
    public class GeneralUserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public GeneralUserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? branchId)
        {
            // Get all branches for the dropdown filter
            var branches = await _dbContext.Branches.ToListAsync();
            ViewBag.Branches = branches;

            // Get activities, filter by branch if branchId is provided
            var activitiesQuery = _dbContext.Activities.Include(a => a.Branch).AsQueryable();
            if (branchId.HasValue)
            {
                activitiesQuery = activitiesQuery.Where(a => a.BranchID == branchId.Value);
            }

            var activities = await activitiesQuery.ToListAsync();
            return View(activities);
        }
    }
}
