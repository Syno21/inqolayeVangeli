using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly ApplicationDbContext _dbContext;

        public ActivityController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult AddActivity()
        {
            var users = _dbContext.Users.ToList();
            var branches = _dbContext.Branches.ToList();
            ViewBag.Users = users;
            ViewBag.Branches = branches;
            var model = new AddActivityViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddActivity(AddActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newActivity = new Activity
                {
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                   // UserID = model.UserID,
                    BranchID = model.BranchID,
              
                };

                _dbContext.Activity.Add(newActivity);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to the home page or any other page
            }

            var users = _dbContext.Users.ToList();
            var branches = _dbContext.Branches.ToList();
            ViewBag.Users = users;
            ViewBag.Branches = branches;
            return View(model);
        }

        public IActionResult ManageActivities()
        {
            var activities = _dbContext.Activity
            .Include(a => a.Branch) // Include the related Branch entity
                            // .Find(a => a.Branch)
                .ToList();
            return View(activities);
        }

      

        public IActionResult EditActivity(int id)
        {
            var activity = _dbContext.Activity
                .FirstOrDefault(a => a.ActivityID == id);

            if (activity == null)
            {
                return NotFound();
            }

            var model = new AddActivityViewModel
            {
                ActivityID = activity.ActivityID,
                Name = activity.Name,
                Description = activity.Description,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
               // UserID = activity.UserID,
                BranchID = activity.BranchID
            };

            LoadViewBagData();
            return View("AddActivity", model);
        }

        [HttpPost]
        public IActionResult SaveActivity(AddActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ActivityID == 0)
                {
                    // Create new activity
                    var newActivity = new Activity
                    {
                        Name = model.Name,
                        Description = model.Description,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                      //  UserID = model.UserID,
                        BranchID = model.BranchID
                    };
                    _dbContext.Activity.Add(newActivity);
                }
                else
                {
                    // Update existing activity
                    var existingActivity = _dbContext.Activity
                        .FirstOrDefault(a => a.ActivityID == model.ActivityID);

                    if (existingActivity == null)
                    {
                        return NotFound();
                    }

                    existingActivity.Name = model.Name;
                    existingActivity.Description = model.Description;
                    existingActivity.StartDate = model.StartDate;
                    existingActivity.EndDate = model.EndDate;
                   // existingActivity.UserID = model.UserID;
                    existingActivity.BranchID = model.BranchID;

                    _dbContext.Activity.Update(existingActivity);
                }

                _dbContext.SaveChanges();
                return RedirectToAction("ManageActivities");
            }

            LoadViewBagData();
            return View("AddActivity", model);
        }

        public IActionResult DeleteActivity(int id)
        {
            var activity = _dbContext.Activity.FirstOrDefault(a => a.ActivityID == id);

            if (activity == null)
            {
                return NotFound();
            }

            _dbContext.Activity.Remove(activity);
            _dbContext.SaveChanges();

            return RedirectToAction("ManageActivities");
        }

        private void LoadViewBagData()
        {
            ViewBag.Users = _dbContext.Users.ToList();
            ViewBag.Branches = _dbContext.Branches.ToList();
        }
    }
}