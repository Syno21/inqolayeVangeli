using System.ComponentModel.DataAnnotations;

namespace InqolaYevangeli.Models
{
    public class AddBranchViewModel
    {

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



    }
}
