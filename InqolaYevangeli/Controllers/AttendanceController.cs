using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Controllers
{
    public class AttendanceController : Controller
    {
     

        private readonly ApplicationDbContext _dbContext;

        public AttendanceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            var activities = _dbContext.Activities.ToList();
            ViewBag.Activities = activities;

            return View();
        }

        public IActionResult ActivityDetails()
        {

            var activities = _dbContext.Activities.ToList();
            ViewBag.Activities = activities;

            return View();
        }


      
        public IActionResult RegisterAttendance(AttendanceViewModel model)
        {
             var activities = _dbContext.Activities.ToList();
            ViewBag.Activities = activities;
            // Attempt to parse memberId to int
            if (!int.TryParse(model.MemberID, out int memberIdInt))
            {
                // Handle invalid memberId format
                ModelState.AddModelError("MemberId", "Invalid Member ID format.");
                return View("ActivityDetails", model);
            }

            // Check if the member ID exists
            if (!MemberExists(memberIdInt))
            {
                ModelState.AddModelError("MemberId", "Member ID does not exist.");
                return View("ActivityDetails", model);
            }

            // Proceed with attendance registration
            // Here, you need to get the selected activity ID from the model
            int activityId = model.ActivityID;

            // Example: Create an attendance record
            var attendance = new Attendance
            {
                ActivityID = activityId,
                MemberID = memberIdInt,
                DateAttended = DateTime.Now // Or any other date you prefer
            };

            _dbContext.Attendance.Add(attendance);
            _dbContext.SaveChanges();

            // Redirect back to the activity details page or any other appropriate page
            return RedirectToAction("ActivityDetails");
        }

        private bool MemberExists(int memberId)
        {
            return _dbContext.Members.Any(m => m.MemberID == memberId);
        }

    }
}

