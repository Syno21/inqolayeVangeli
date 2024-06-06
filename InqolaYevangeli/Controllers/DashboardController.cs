using InqolaYevangeli.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace InqolaYevangeli.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _dbContext;

        public DashboardController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Dashboard()
        {
            // Retrieve membership statistics
            int totalMembers = _dbContext.Members.Count();
            int activeMembers = _dbContext.Members.Count();
           // int activeMembers = _dbContext.Members.Where(m => m.IsActive).Count();
            int inactiveMembers = totalMembers - activeMembers;



            // Retrieve number of branches in each province
            var branchesInProvince = _dbContext.Branches
                .GroupBy(b => b.State)
                .Select(group => new { Province = group.Key, Count = group.Count() })
                .ToList();

            // Membership trends data
            var membershipTrends = _dbContext.Members.ToList();

            var chartLabels = branchesInProvince.Select(bp => bp.Province).ToList();
            var chartData = branchesInProvince.Select(bp => bp.Count).ToList();
            // Retrieve total number of branches in the country
            int totalBranches = _dbContext.Branches.Count();

            ViewBag.TotalMembers = totalMembers;
            ViewBag.ActiveMembers = activeMembers;
            ViewBag.InactiveMembers = inactiveMembers;
            ViewBag.Users = _dbContext.Users.Count();
            ViewBag.BranchesInProvince = branchesInProvince;
            ViewBag.TotalBranches = totalBranches;
            ViewBag.ChartLabels = string.Join(",", chartLabels.Select(label => $"'{label}'"));
            ViewBag.ChartData = string.Join(",", chartData);
            ViewBag.ChartLabels = chartLabels;
            ViewBag.ChartData = chartData;

            // Retrieve membership trends data (example)



            return View();
        }
    }
}
