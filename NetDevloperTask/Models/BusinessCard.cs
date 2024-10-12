using System.ComponentModel.DataAnnotations;

namespace NetDevloperTask.Models
{
    public class BusinessCard
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public string Photo { get; set; } // Optional
    }

}
