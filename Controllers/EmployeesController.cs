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
    public class EmployeesController : Controller
    {
        private readonly StudentRecordContext _context;

        public EmployeesController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var studentRecordContext = _context.AcademicRecords.Include(a => a.CourseCodeNavigation).Include(a => a.Student);
            
            var employeeRolesContext= _context.Employees.Include(a => a.Roles);
            var employee_Roles = await _context.Roles.ToListAsync();

            return View(await employeeRolesContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            var roleContext = _context.Roles;
            ViewData["Roles"] = await roleContext.ToListAsync();

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            string[] selectedRoles = Request.Form["checkBoxList"];

            var roles = _context.Roles.Where(r => selectedRoles.Contains(r.Role1));
            employee.Roles  = await  roles.ToListAsync();

            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var roleContext = _context.Roles.Include(a=> a.Employees);
            ViewData["Roles"] = await roleContext.ToListAsync();
            ViewData["EmployeeId"] = id.ToString();
            List<Employee> employees = new List<Employee>();
            employees.Add(employee);    
            ViewData["Employee"]= employees;

            var employeeRolesContext = _context.Employees.Include(a => a.Roles);
            List<Role> Roles = new List<Role>();
            
            foreach (var emp in employees)
            {
                foreach(var role in emp.Roles)
                {
                    Roles.Add(role);
                }
            }

            ViewData["Employee_Roles"] = Roles;

            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {

            string[] selectedRoles = Request.Form["checkBoxList"];
            var roles = _context.Roles.Where(r => selectedRoles.Contains(r.Role1));
            var updatedRoles = await roles.ToListAsync();
            var dbEmployee = await _context.Employees.Include(e => e.Roles).FirstOrDefaultAsync(e => e.Id == employee.Id);

            dbEmployee.Name = employee.Name;
            dbEmployee.UserName= employee.UserName;
            dbEmployee.Password = employee.Password;
            dbEmployee.Roles = new HashSet<Role>(updatedRoles);
            employee.Roles = await roles.ToListAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbEmployee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'StudentRecordContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.Id == id);
        }
    }
}
