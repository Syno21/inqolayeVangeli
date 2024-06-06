using System.ComponentModel.DataAnnotations;

namespace InqolaYevangeli.Models
{
    public class LoginViewModel
    {
 

        //[Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        //[Required]
        [StringLength(20)]
        public string? Role { get; set; }
    }
}
