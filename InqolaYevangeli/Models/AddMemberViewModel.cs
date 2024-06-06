using System.ComponentModel.DataAnnotations;

namespace InqolaYevangeli.Models
{
    public class AddMemberViewModel
    {
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

        public int BranchID { get; set; } = 0;
        // public string Branch { get; set; }

       
        public int StatusID { get; set; }
        //public string StatusName { get; set; }


    }
   
}
