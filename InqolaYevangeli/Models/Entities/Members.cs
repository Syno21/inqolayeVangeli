using System.ComponentModel.DataAnnotations;

namespace InqolaYevangeli.Models.Entities
{
    public class Members
    {


        // User entity class
        public class User
        {
            [Key]
            public int UserID { get; set; }

            [Required]
            [StringLength(50)]
            public string Username { get; set; }

            [Required]
            [StringLength(100)]
            public string Password { get; set; }

            [Required]
            [StringLength(20)]
            public string Role { get; set; }

            public int? BranchID { get; set; } // Nullable int to represent the BranchID
            public Branch Branch { get; set; }
        }

        // Branch entity class
        public class Branch
        {
            [Key]
            public int BranchID { get; set; }

            [Required]
            [StringLength(100)]
            public string BranchName { get; set; }

            [Required]
            [StringLength(100)]
            public string Location { get; set; }


            [Required]
            [StringLength(100)]
            public string Country { get; set; }

            [Required]
            [StringLength(100)]
            public string State { get; set; }

            public ICollection<Member> Members { get; set; }


        }

        // Member entity class
        public class Member
        {
            [Key]
            public int MemberID { get; set; }

            [Required]
            [StringLength(50)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50)]
            public string LastName { get; set; }

            [Required]
            public DateTime DateOfBirth { get; set; }

            [Required]
            [StringLength(10)]
            public string Gender { get; set; }

            public int BranchID { get; set; }
            public Branch Branch { get; set; }

            public ICollection<MemberStatusHistory> StatusHistory { get; set; }
            public ICollection<Attendance> Attendances { get; set; }
            public ICollection<Demographics> Demographics { get; set; }
            public ICollection<ActivityHistory> ActivityHistory { get; set; }
        }

        // MembershipStatus entity class
        public class MembershipStatus
        {
            [Key]
            public int StatusID { get; set; }

           
            [StringLength(50)]
            public string StatusName { get; set; }

            public ICollection<MemberStatusHistory> MemberStatusHistory { get; set; }
        }

        // MemberStatusHistory entity class
        public class MemberStatusHistory
        {
            [Key]
            public int StatusHistoryID { get; set; }

            public int MemberID { get; set; }
            public Member Member { get; set; }

            public int StatusID { get; set; }
            public MembershipStatus Status { get; set; }

            [Required]
            public DateTime DateChanged { get; set; }
        }

        // Attendance entity class
        public class Attendance
        {
            [Key]
            public int AttendanceID { get; set; }

            public int MemberID { get; set; }
            public Member Member { get; set; }

            [Required]
            public DateTime DateAttended { get; set; }

            public int ActivityID { get; set; }
        }

        // Demographics entity class
        public class Demographics
        {
            [Key]
            public int DemographicsID { get; set; }

            public int MemberID { get; set; }
            public Member Member { get; set; }

            [StringLength(20)]
            public string AgeGroup { get; set; }


        }

        // ActivityHistory entity class
        public class ActivityHistory
        {
            [Key]
            public int ActivityID { get; set; }

            public int MemberID { get; set; }
            public Member Member { get; set; }

            [StringLength(50)]
            public string ActivityType { get; set; }

            public DateTime DateOfActivity { get; set; }
        }

        public class Activity
        {
            [Key]
            public int ActivityID { get; set; }

            [Required]
            [StringLength(50)]
            public string Name { get; set; }

            [Required]
            [StringLength(200)]
            public string Description { get; set; }

            [Required]
            public DateTime Date { get; set; }

           // public int UserID { get; set; }
           // public Members.User? User { get; set; }  // Navigation property
            public DateTime EndDate { get; internal set; }
            public DateTime StartDate { get; internal set; }
            public int BranchID { get; set; }
            public Branch Branch { get; set; }
        }

    }
}
