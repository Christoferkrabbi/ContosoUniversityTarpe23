using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Courses;
            return View(await schoolContext.ToListAsync());
        }

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var course = await _context.Courses
        //        .FirstOrDefaultAsync(m => m.CourseID == id);
        //    if (course == null) { return NotFound(); }

        //    ViewData["Action"] = "Details";
        //    return View("DetailsDelete", course);
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var course = await _context.Courses
        //        .FirstOrDefaultAsync(m => m.CourseID == id);

        //    if (course == null) { return NotFound(); }

        //    ViewData["Action"] = "Delete";
        //    return View("DetailsDelete", course);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var course = await _context.Courses.FindAsync(id);
        //    _context.Courses.Remove(course);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> Clone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }



            var maxId = await _context.Courses.MaxAsync(c => (int?)c.CourseID) ?? 0;
            var newCourseId = maxId + 1;

           
            while (await _context.Courses.AnyAsync(c => c.CourseID == newCourseId))
            {
                newCourseId++;
            }

            var clonedCourse = new Course
            {
                CourseID = newCourseId, 
                Title = course.Title,
                Credits = course.Credits
            };

            _context.Courses.Add(clonedCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", course.CourseID);
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Fetch the department by id including its related data if necessary
            var department = await _context.Departments
                .Include(d => d.Administrator) // Include related data if needed
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }
            // Pass the Instructor list for dropdown
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }
        // Edit POST

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID, Title, Credits")] Course course)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing department from the database
                    var existingCourse = await _context.Courses
                        .FirstOrDefaultAsync(d => d.CourseID == id);
                    if (existingCourse == null)
                    {
                        return NotFound();
                    }
                    // Update the department fields manually
                    existingCourse.Title = course.Title;
                    existingCourse.Credits = course.Credits;
                   

                    // Update the InstructorID (foreign key)
                    existingCourse.CourseID = course.CourseID;
                    // Save the changes
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Courses.Any(d => d.CourseID == course.CourseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            // Reload the dropdown list and return to the view if model state is invalid
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", course.CourseID);
            return View(course);
        }
       

    }
}