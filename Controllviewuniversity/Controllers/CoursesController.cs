using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [HttpGet]
        public async Task<IActionResult> DetailsDelete(int? id, string mode)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseID == id);

            if (course == null)
            {
                return NotFound();
            }

            ViewBag.Mode = mode;

            return View(course);
        }

        [HttpPost, ActionName("DetailsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsDeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //public async Task<IActionResult> Clone(int id)
        //{
        //    var course = await _context.Courses
        //        .FirstOrDefaultAsync(c => c.CourseID == id);

        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    var maxCourseID = await _context.Courses.MaxAsync(c => c.CourseID);
        //    var newCourseID = maxCourseID + 1;

        //    var clonedCourse = new Course
        //    {
        //        CourseID = newCourseID,
        //        Title = course.Title,
        //        Credits = course.Credits
        //    };

        //    _context.Courses.Add(clonedCourse);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            if (id != null)
            {
                ViewBag.mode2 = "edit";
                var course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.CourseID == id);

                if (course == null)
                {
                    return NotFound();
                }

				System.Diagnostics.Debug.WriteLine("This is a log");
				System.Console.WriteLine(course);
                return View(course);
            }
            else
            {
                ViewBag.mode2 = "create";

                return View();
            }
        }

        [HttpPost, ActionName("CreateEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdit(int? id, [Bind("CourseID,Title,Credits")] Course course)
        {
            if (id != null)
            {
                ViewBag.mode2 = "edit";
            }
            else
            {
                ViewBag.mode2 = "create";
            }
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    var existingCourse = await _context.Courses
                    .FirstOrDefaultAsync(c => c.CourseID == id);

                    if (existingCourse == null)
                    {
                        return NotFound();
                    }

                    if (course.CourseID < 0)
                    {
                        ModelState.AddModelError("CourseID", "CourseID is negative. Please enter a positive CourseID.");
                        return View(course);
                    }

                    if (_context.Courses.Any(c => c.CourseID == course.CourseID))
                    {
                        if (course.CourseID == existingCourse.CourseID)
                        {
                            existingCourse.CourseID = course.CourseID;
                            existingCourse.Title = course.Title;
                            existingCourse.Credits = course.Credits;

                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }
                        // Add a validation error to the ModelState
                        ModelState.AddModelError("CourseID", "CourseID already exists. Please enter an unique CourseID.");
                        return View(course); // Return the same view with the error message
                    }

                    _context.Courses.Remove(existingCourse);
                    _context.Courses.Add(course);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (_context.Courses.Any(c => c.CourseID == course.CourseID))
                    {
                        // Add a validation error to the ModelState
                        ModelState.AddModelError("CourseID", "CourseID already exists. Please enter an unique CourseID.");
                        return View(course); // Return the same view with the error message
                    }
                    if (course.CourseID < 0)
                    {
                        ModelState.AddModelError("CourseID", "CourseID is negative. Please enter a positive CourseID.");
                        return View(course);
                    }
                    _context.Courses.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(course);
        }


    }
}