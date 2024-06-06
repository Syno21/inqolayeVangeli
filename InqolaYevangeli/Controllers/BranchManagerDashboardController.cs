using InqolaYevangeli.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Controllers
{
    public class BranchManagerDashboardController : Controller
    {

        private ApplicationDbContext _dbContext;

        public BranchManagerDashboardController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActionResult ManagerDashboard()
        {
            // Retrieve the BranchID claim from the current user's claims
            var branchIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BranchID");

            if (branchIdClaim != null && int.TryParse(branchIdClaim.Value, out int branchId))
            {
                // Retrieve branch information based on the BranchID
                var branch = _dbContext.Branches.FirstOrDefault(b => b.BranchID == branchId);

                if (branch != null)
                {
                    // Get the count of members in each age group for the branch
                    var memberCounts = _dbContext.Members
                        .Where(m => m.BranchID == branchId)
                        .SelectMany(m => m.Demographics)
                        .GroupBy(d => d.AgeGroup)
                        .Select(g => new { AgeGroup = g.Key, Count = g.Count() })
                        .ToList();

                    ViewBag.numberOfMembers = _dbContext.Members
                        .Where(m => m.BranchID == branchId).Count();

                    // Example: Query to get the number of sealed members, welcomed members, and testifiers
                    ViewBag.SealedMembers = GetMemberCountByStatus("Sealed", branchId);
                    ViewBag.WelcomedMembers = GetMemberCountByStatus("Welcomed", branchId);
                    ViewBag.Testifiers = GetMemberCountByStatus("Testifier", branchId);

                    // Fetch the number of activities
                    var activities = _dbContext.Activities.Where(a => a.BranchID == branchId).ToList();
                    ViewBag.NumberOfActivities = activities.Count;

                    // Calculate the attendance rate
                    int totalAttendances = _dbContext.Attendance.Count(a => a.Member.BranchID == branchId);
                    int totalMembers = _dbContext.Members.Count(m => m.BranchID == branchId);
                    ViewBag.AttendanceRate = totalMembers > 0 ? (double)totalAttendances / totalMembers * 100 : 0;

                    // Use branch data and member counts to display information on the dashboard
                    ViewBag.BranchName = branch.BranchName;
                    ViewBag.Location = branch.Location;
                    ViewBag.MemberCounts = memberCounts;

                    return View();
                }
                else
                {
                    // Handle case where branch is not found
                    ViewBag.ErrorMessage = "Branch not found.";
                }
            }
            else
            {
                // Handle case where BranchID claim is not found or not a valid integer
                ViewBag.ErrorMessage = "Invalid or missing Branch ID claim.";
            }

            // Redirect to an error page or display an error message
            return View("Error");
        }

        private int GetMemberCountByStatus(string statusName, int branchId)
        {
            return _dbContext.MemberStatusHistory
                .Where(msh => msh.Member.BranchID == branchId && msh.Status.StatusName == statusName)
                .Select(msh => msh.MemberID)
                .Distinct()
                .Count();
        }

    }
}
