using System.ComponentModel.DataAnnotations;

namespace InqolaYevangeli.Models
{
    public class AddActivityViewModel
    {

        [Required]
        public int ActivityID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        //[Required]
       // public TimeSpan Time { get; set; }

      //  [Required]
       // public int UserID { get; set; }

        [Required]
        public int BranchID { get; set; }
      
    }

    public class EditActivityViewModel
    {
        [Required]
         public int ActivityID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        //[Required]
        //public TimeSpan Time { get; set; }

        //[Required]
       // public int UserID { get; set; }

        [Required]
        public int BranchID { get; set; }
    }
}


