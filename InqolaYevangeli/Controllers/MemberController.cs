using InqolaYevangeli.data;
using InqolaYevangeli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static InqolaYevangeli.Models.Entities.Members;


namespace InqolaYevangeli.Controllers
{
    public class MemberController : Controller
    {
        private ApplicationDbContext dbContext;

        public MemberController(ApplicationDbContext dbContext)
        {
            this.dbContext=dbContext;
        }

       

        [HttpGet]
        public IActionResult AddMember()
        {
            var branches = dbContext.Branches.ToList();
            ViewBag.Branches = branches;

            var membershipStatuses = dbContext.MembershipStatuses.ToList();
            ViewBag.MembershipStatuses = membershipStatuses;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var member = await dbContext.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            // Assuming you need to populate dropdowns for edit, similar to AddMember action
            var branches = await dbContext.Branches.ToListAsync();
            ViewBag.Branches = branches;

            var membershipStatuses = await dbContext.MembershipStatuses.ToListAsync();
            ViewBag.MembershipStatuses = membershipStatuses;

            // Find the latest status change record for the given member ID
            var latestStatusChange = await dbContext.MemberStatusHistory
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




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(AddMemberViewModel memberData)
        {
          
            if (!ModelState.IsValid)
            {
                // If model state is not valid, repopulate ViewBag data and return the view with validation errors
                var branches = dbContext.Branches.ToList();
                ViewBag.Branches = branches;

                var membershipStatuses = dbContext.MembershipStatuses.ToList();
                ViewBag.MembershipStatuses = membershipStatuses;

                // Return the view with validation errors and repopulated ViewBag data
                return View(memberData);
            }

            // Check if the member already exists in the database
            var existingMember = await dbContext.Members.FirstOrDefaultAsync(m =>
                m.FirstName == memberData.FirstName &&
                m.LastName == memberData.LastName &&
                m.DateOfBirth == memberData.DateOfBirth);


            Member newMember = null;

            if (existingMember != null)
            {
                // Update existing member's information
                existingMember.FirstName = memberData.FirstName;
                existingMember.LastName = memberData.LastName;
                existingMember.DateOfBirth = memberData.DateOfBirth;
                existingMember.Gender = memberData.Gender;
                existingMember.BranchID = memberData.BranchID;
                // You might need to update other properties here

                dbContext.Members.Update(existingMember);
            }
            else
            {
                // Add the new member to the database
                newMember = new Member
                {
                    FirstName = memberData.FirstName,
                    LastName = memberData.LastName,
                    DateOfBirth = memberData.DateOfBirth,
                    Gender = memberData.Gender,
                    BranchID = memberData.BranchID
                };

                dbContext.Members.Add(newMember);
            }

            // Save changes to the database
            await dbContext.SaveChangesAsync();

            // Get the MemberID of the newly added member or the updated member
            int memberID = (existingMember != null) ? existingMember.MemberID : newMember.MemberID;


            // Record the status change in the MemberStatusHistory table
            var statusChange = new MemberStatusHistory
            {
                MemberID = memberID,
                StatusID = memberData.StatusID,
                DateChanged = DateTime.Now
            };

            // Add the status change to the MemberStatusHistory table
            await dbContext.MemberStatusHistory.AddAsync(statusChange);
            await dbContext.SaveChangesAsync();

            // Determine the age group of the member
            string ageGroup = CalculateAgeGroup(memberData.DateOfBirth);

            var newDemographicsEntry = new Demographics
            {
                MemberID = memberID,
                AgeGroup = ageGroup
            };

            // Add the new demographics entry to the database
            dbContext.Demographics.Add(newDemographicsEntry);
            await dbContext.SaveChangesAsync();

            // After processing the form submission, redirect to the same action method which will reload the form
            return RedirectToAction(nameof(AddMember));
        }


        private string CalculateAgeGroup(DateTime dateOfBirth)
        {
            // Calculate the age of the member
            int age = DateTime.Today.Year - dateOfBirth.Year;

            // Check if the member has already celebrated their birthday this year
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
                age--;

            // Determine the age group based on the age
            if (age <= 15)
            {
                return "Sunday School";
                
            }
            else if (age > 15 && age <= 24)
            {
                return "Youth";
            }
            else
            {
                return "Adult";
            }
        }
    }
}
