using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Models
{
    public class InstructorIndexdata : Controller
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
