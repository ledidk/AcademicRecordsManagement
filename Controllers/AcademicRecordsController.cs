using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab06.Models.DataAccess;
using Lab7.Models.DataAccess;

namespace Lab06.Controllers
{
    public class AcademicRecordsController : Controller
    {
        private readonly StudentRecordContext _context;
        public List<AcademicRecord> academicRecords;

        public AcademicRecordsController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: AcademicRecords
        public async Task<IActionResult> Index()
        {

            //Created a class that will hold all of the database data - easy to bind to
            //the form as class share id but not all data we need to display on the form
            var studentRecordContext =  _context.AcademicRecords.Include(a => a.CourseCodeNavigation).Include(a => a.Student);
            List<AcademicRecord> academicRecordsList = _context.AcademicRecords.ToList();
            List<Course> coursesList = _context.Courses.ToList();
            List<Student> studentsList = _context.Students.ToList();
            List<Employee> employeesList = _context.Employees.ToList();
            List<Role> rolesList = _context.Roles.ToList();

            AllRecords allStudentsRecords = new Lab7.Models.DataAccess.AllRecords
            {
                AcademicRecordsList=academicRecordsList,
                CoursesList = coursesList,
                StudentsList=studentsList,
                RolesList =rolesList,
                EmployeesList = employeesList
            };

            return View(await studentRecordContext.ToListAsync());
        }

        // GET: AcademicRecords/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AcademicRecords == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // GET: AcademicRecords/Create
        public IActionResult Create()
        {
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: AcademicRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/EditAll/
        public async Task<IActionResult> EditAll()
        {
            var studentRecordContext = _context.AcademicRecords.Include(a => a.CourseCodeNavigation).Include(a => a.Student);

            return View(await studentRecordContext.ToListAsync());
        }

        // GET: AcademicRecords/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            List<AcademicRecord> academicRecords = new List<AcademicRecord>();
            if (id == null && _context.AcademicRecords == null)
            {
                return NotFound();
            }

            var studentRecordContext = _context.AcademicRecords.Include(a => a.CourseCodeNavigation).Include(a => a.Student);
            if (id !=null)
            {

            }
            try { 
                var academicRecord = studentRecordContext.Single(m => m.StudentId == id);
                ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
                ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            } catch { };


            //return View(await studentRecordContext.ToListAsync());
            return View(await studentRecordContext.ToListAsync());
        }

        // POST: AcademicRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost , ActionName("Edit")]
        public async Task<IActionResult> Edit(List<AcademicRecord> academicRecord)
        {

            using (var context = new StudentRecordContext())
            {
                var academicRecords = await _context.AcademicRecords.ToListAsync();
                foreach (var record in academicRecord)
                {
                    var dbacademicRecord = academicRecords.FirstOrDefault(a => a.StudentId.Equals(record.StudentId) && a.CourseCode.Equals(record.CourseCode));
                    if(dbacademicRecord != null)
                    {
                        dbacademicRecord.Grade = record.Grade;
                    }
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");

        }

        // GET: AcademicRecords/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AcademicRecords == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // POST: AcademicRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AcademicRecords == null)
            {
                return Problem("Entity set 'StudentRecordContext.AcademicRecords'  is null.");
            }
            var academicRecord = await _context.AcademicRecords.FindAsync(id);
            if (academicRecord != null)
            {
                _context.AcademicRecords.Remove(academicRecord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicRecordExists(string id)
        {
          return _context.AcademicRecords.Any(e => e.StudentId == id);
        }
    }
}
