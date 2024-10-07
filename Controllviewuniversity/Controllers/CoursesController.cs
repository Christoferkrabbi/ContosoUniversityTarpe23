using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
         
    {
        private readonly SchoolContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //string query = "SELECT * FROM Departments WHERE DepartmentID = {0}";
            //var department = await _context.Courses
            //    .FromSqlRaw(query, id)
            //    .Include(d => d.CourseID)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync();
            //if (department == null)
            //{
            //    return NotFound();
            //}
            //return View(department);

            var Course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (Course == null)
            {
                return NotFound();
            }

            return View(Course);
        }


    }
}
