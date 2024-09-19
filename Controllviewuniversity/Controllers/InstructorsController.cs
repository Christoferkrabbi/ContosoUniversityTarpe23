using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            var vm = new InstructorIndexData();
            vm.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Enrollments)
                .ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = vm.Instructors
                    .Where(i => i.ID == id.Value).Single();
                vm.Courses = instructor.CourseAssignments
                    .Select(i => i.Course);

            }
            if (courseId != null)
            {
                ViewData["CourseID"] = courseId.Value;
                vm.Enrollments = vm.Courses
                    .Where(x => x.CourseID == courseId.Value)
                    .Single()
                    .Enrollments;
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor) //string selectedCourses)
        {
           
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //PopulateAssignedCourseData(instructor); //uuendab instructori juures olevaid kursuseid
            return View(instructor);
        }
        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses; // kõik kursused
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            //valime kursused kus courseid on õpetajal olemas
            var vm = new List<AssignedCourseData>(); 
            foreach (var course in allCourses)
            {
                vm.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewData["Courses"] = vm;

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) //Kui id on tühi, siis õpilast ei leita
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FirstOrDefaultAsync(M => M.ID == id); //tehakse õpilase objekt admebaasis oleva id järgi

            if (instructor == null) //Kui student objekt on tühi/null siis ka õpilast ei leia
            {
                return NotFound();
            }

            return View(instructor);


        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id); //otsime andmebaasist õpilast id järgi ja paneme ta students nimelisse objekti voi muutujasse
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        
        private bool InstructorsExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstMidName,HireDate")] Instructor instructor)
        {
            if (id != instructor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(instructor);
                await _context.SaveChangesAsync();
            }
            if (!InstructorsExists(instructor.ID))
            {
                return NotFound();
            }


            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Clone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            var clonedinstructor = new Instructor
            {
                FirstMidName = instructor.FirstMidName,
                LastName = instructor.LastName,
                HireDate = instructor.HireDate
            };

            _context.Instructors.Add(clonedinstructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
