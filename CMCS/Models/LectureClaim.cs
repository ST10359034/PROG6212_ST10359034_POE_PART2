/*using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Models
{
    public class LectureClaim
    {
        [Key]
        public int ClaimID { get; set; }

        [Required]
        public string LecturerName { get; set; }

        [Required]
        public string LecturerSurname { get; set; }

        [Required]
        [EmailAddress]
        public string LecturerEmail { get; set; }

        [DataType(DataType.Date)]
        public DateTime ClaimDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid hours worked")]
        public decimal HoursWorked { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid hourly rate")]
        public decimal HourlyRate { get; set; }

      

        [NotMapped]
        [Display(Name = "Upload Supporting Document")]
        public IFormFile ClaimDocument { get; set; }
    }
}
*/

using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Models
{
    public class LectureClaim
    {
        [Key]
        public int ClaimID { get; set; }

        [Required]
        public string LecturerName { get; set; }

        [Required]
        public string LecturerSurname { get; set; }

        [Required]
        [EmailAddress]
        public string LecturerEmail { get; set; }

        [DataType(DataType.Date)]
        public DateTime ClaimDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid hours worked")]
        public decimal HoursWorked { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid hourly rate")]
        public decimal HourlyRate { get; set; }

        // New field for status, with default value 'Pending'
        [Required]
        public string Status { get; set; } = "Pending";

       

        [NotMapped]
        [Display(Name = "Upload Supporting Document")]
        public IFormFile ClaimDocument { get; set; }
    }
}
