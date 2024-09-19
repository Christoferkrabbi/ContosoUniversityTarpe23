using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : Controller
    {

        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }
       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        /*

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber
            )
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NamesortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSort"] = sortOrder == "Date" ? "date_desc" : "Date";


            if (searchString is null)
            {
                pageNumber = 1;
            }
            else 
            {
                searchString = currentFilter;
            }
            ViewData["currentFilter"] = searchString;

            var students = from student in _context.Students
                           select student;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(student =>
                student.LastName.Contains(searchString) ||
                student.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
               students = students.OrderByDescending(student => student.LastName);
                break;
                case "Firstname_desc":
               students = students.OrderByDescending(student => student.FirstMidName);
                    break;
                case "Date":
               students = students.OrderBy(student => student.EnrollmentDate);
                    break;
                case "date_desc":
               students = students.OrderByDescending(student => student.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(student => student.LastName);
                    break;

            }

            int pageSize = 3;
            return View(await _context.Students.ToListAsync()); 
            
        } */
        //Create get, haarab vaatest andmed, mida create meetod vajab.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create meetod, sisestab andmebaasi uue õpilase. Insert new studnet into database

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        //Delete GET meetod, otsiv andmebaasist kaasaantud id järgi õpilast.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) //Kui id on tühi, siis õpilast ei leita
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(M => M.ID == id); //tehakse õpilase objekt admebaasis oleva id järgi

            if (student == null) //Kui student objekt on tühi/null siis ka õpilast ei leia
            {
                return NotFound();
            }

            return View(student);


        }
        //Delete POST meetod, teostab andmebaasi vajaliku muudatuse. ehk kustutab objekti ära

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id); //ostsime andmebaasist õpilast id järgi ja paneme ta students nimelisse objekti voi muutujasse
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null) //Kui id on tühi, siis õpilast ei leita
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(M => M.ID == id); //tehakse õpilase objekt admebaasis oleva id järgi

            if (student == null) //Kui student objekt on tühi/null siis ka õpilast ei leia
            {
                return NotFound();
            }

            return View(student);

        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        [HttpPost, ActionName("ViewDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewDetailsPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            
            return View("Details", student);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            if (!StudentExists(student.ID))
            {
                return NotFound();
            }


            return RedirectToAction(nameof(Index));

        }


        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }

        public async Task<IActionResult> Clone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }


            var clonedstudent = new Student
            {
                FirstMidName = student.FirstMidName,
                LastName = student.LastName,
                EnrollmentDate = student.EnrollmentDate
            };

            _context.Students.Add(clonedstudent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }
    }
}


