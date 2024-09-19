using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Instructor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName 
        { get 
            { return LastName + ", " + FirstMidName; } 
        }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name ="Hired on:")]
        public DateTime HireDate { get; set; }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }
        public OfficeAssignment? OfficeAssignment { get; set; } 

        public int? Age { get; set; } //vanus
                                      //
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Birthday on: ")]
        public int? BirthDay { get; set; } //sünnipäev

        public Mood? Mood { get; set; } //õpetaja tuju
        [Display(Name = "Kutsetunnistuse #:")]
        public string? VocationCredential { get; set; }
        
    }

    public enum Mood
    {
        Happy, Sad, Anxious, Puzzled, HighAF
    }



}
