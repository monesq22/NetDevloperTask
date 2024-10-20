using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace NetDevloperTask.Models
{
    public class BusinessCard
    {
        [XmlIgnore]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
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
