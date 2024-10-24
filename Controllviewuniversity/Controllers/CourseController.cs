﻿using ContosoUniversity.Data;
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
        public async Task<IActionResult> Details(int? id)
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
            return View(course);
        }
		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
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
			return View(course);
		}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var course = await _context.Courses.FindAsync(id);
			_context.Courses.Remove(course);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Clone(int id)
		{
			var course = await _context.Courses
				.FirstOrDefaultAsync(c => c.CourseID == id);
			if (course == null)
			{
				return NotFound();
			}
			var maxCourseID = await _context.Courses.MaxAsync(c => c.CourseID);
			var newCourseID = maxCourseID + 1;
			var clonedCourse = new Course
			{
				CourseID = newCourseID,
				Title = course.Title,
				Credits = course.Credits
			};
			_context.Courses.Add(clonedCourse);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
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
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CourseID,Title,Credits")] Course course)
		{
			if (ModelState.IsValid)
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
			return View(course);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
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
			return View(course);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CourseID,Title,Credits")] Course course)
		{
			if (ModelState.IsValid)
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
			return View(course);
		}
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