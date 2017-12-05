using System.ComponentModel.DataAnnotations;

namespace final.Models
{
    public class Contact
    {
        [Required]
        public string Name { get; set; }

       [Required]
       [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Note { get; set; }
    }
}