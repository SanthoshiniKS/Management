using Management.Data;
using Management.Models;
using Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var employees =await _context.Employees.Include(e => e.Department).Include(e => e.City)
                    .ToListAsync();
                return View(employees);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An Error occured while fetching data: " + ex.Message;
                return View(new List<Employee>());
            }
        }

        public async Task<IActionResult> Add()
        {
            try
            {
                ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
                ViewBag.Cities = new SelectList(_context.Cities, "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Employee emp, string? otherCity)
        {
            try
            {
                //Check any error
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"{state.Key}:{error.ErrorMessage}");
                        }
                    }
                }

                //check whether the employee is a Manager
                if (string.Equals(emp.Position, "Manager", StringComparison.OrdinalIgnoreCase))
                {
                    emp.Position = "Manager";
                }
                if (emp.ManagerID.HasValue && emp.Position == "Manager")
                {
                    var existingManager =await _context.Employees.FirstOrDefaultAsync(e => e.DepartmentId == emp.DepartmentId && e.ManagerID == null);
                    if (existingManager != null)
                    {
                        return Json(new { success = false, message = "This Department already has a Manager" });
                    }
                }

                //If new city comes
                if (!string.IsNullOrEmpty(otherCity))
                {
                    var newCity = new City { Name = otherCity };
                    var exists =await _context.Cities.FirstOrDefaultAsync(c => c.Name == otherCity);
                    if (exists != null) { return Json(new { success = false, message = "City already exists" }); }
                    _context.Cities.Add(newCity);
                    _context.SaveChangesAsync();

                    var saved =await _context.Cities.FirstOrDefaultAsync(c => c.Name == otherCity);
                    if (saved != null)
                    {
                        var newCityId = saved.Id;
                        emp.CityId = newCityId;
                    }
                    //emp.CityId = newCity.Id;
                }

                ModelState.Clear();
                TryValidateModel(emp);
                if (ModelState.IsValid)
                {
                    _context.Employees.Add(emp);
                    _context.SaveChangesAsync();

                    //If the new employee is manager, update employees without manager id in same department
                    if (emp.Position == "Manager")
                    {
                        var empToUpdate =await _context.Employees.Where(e => e.DepartmentId == emp.DepartmentId && (e.ManagerID == null || e.ManagerID == 0) && e.Id != emp.Id && e.Position != "CEO")
                            .ToListAsync();
                        foreach (var e in empToUpdate)
                        {
                            e.ManagerID = emp.Id;
                        }
                        _context.SaveChangesAsync();
                    }
                    return Json(new { success = true, message = "Employee Added Successfully" });
                }
                return Json(new { success = false, message = "Validation failed" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Console.WriteLine(id);
                var emp =await _context.Employees.Include(e => e.Department).Include(e => e.City).FirstOrDefaultAsync(e => e.Id == id);

                if (emp == null)
                {
                    return NotFound();
                }
                ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
                ViewBag.Cities = new SelectList(_context.Cities, "Id", "Name");
                Console.WriteLine(ViewBag.Departments);
                return View(emp);
            }
            catch (Exception ex)
            {
                return View("Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee emp, string? otherCity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"{state.Key}:{error.ErrorMessage}");
                        }
                    }
                }
                if (string.Equals(emp.Position, "Manager", StringComparison.OrdinalIgnoreCase))
                {
                    emp.Position = "Manager";
                }
                if (emp.ManagerID.HasValue && emp.Position == "Manager")
                {
                    var existingManager = await _context.Employees.FirstOrDefaultAsync(e => e.DepartmentId == emp.DepartmentId && e.ManagerID == null);
                    if (existingManager != null && existingManager.Id != emp.Id)
                    {
                        return Json(new { success = false, message = "This Department already has a Manager" });
                    }
                }

                Console.WriteLine(emp.CityId);
                if (!string.IsNullOrEmpty(otherCity))
                {
                    var newCity = new City { Name = otherCity };
                    var exists =await _context.Cities.FirstOrDefaultAsync(c => c.Name == otherCity);
                    if (exists != null) { return Json(new { success = false, message = "City already exists" }); }
                    _context.Cities.Add(newCity);
                    Console.WriteLine(newCity.Id);
                    _context.SaveChanges();

                    var saved = _context.Cities.FirstOrDefault(c => c.Name == otherCity);
                    if (saved != null)
                    {
                        var newCityId = saved.Id;
                        Console.WriteLine("----" + newCityId);
                        emp.CityId = newCityId;
                    }
                    //emp.CityId = newCity.Id;
                }
                Console.WriteLine(emp.CityId);
                ModelState.Clear();
                TryValidateModel(emp);

                if (ModelState.IsValid)
                {
                    if (emp.Position == "Manager")
                    {
                        var empToUpdate =await _context.Employees.Where(e => e.DepartmentId == emp.DepartmentId && (e.ManagerID == null || e.ManagerID == 0) && e.Id != emp.Id && e.Position != "CEO")
                            .ToListAsync();
                        foreach (var e in empToUpdate)
                        {
                            e.ManagerID = emp.Id;
                        }
                        _context.SaveChanges();
                    }

                    _context.Employees.Update(emp);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Employee updated successfully" });
                }
                return Json(new { success = false, message = "Validation failed" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            Console.WriteLine(id);
            var empToDelete = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (empToDelete == null)
            {
                return NotFound();
            }
            if (empToDelete.ManagerID == null && empToDelete.Position == "Manager")
            {
                var empUnderManager =_context.Employees.Where(e => e.ManagerID == empToDelete.Id);
                foreach (var emp in empUnderManager)
                {
                    emp.ManagerID = null;
                }
                _context.UpdateRange(empUnderManager);
            }
            _context.Employees.Remove(empToDelete);
            _context.SaveChanges();
            return Json(new { success = true, message = "Employee Deleted Successfully" });
        }


        public async Task<IActionResult> GetManagerByDepartment(int deptId)
        {
            var manager =await _context.Employees.Where(e => e.DepartmentId == deptId && e.Position == "Manager" && e.ManagerID == null)
                .Select(e => new
                {
                    Id = e.Id,
                    Name = e.Name
                }).FirstOrDefaultAsync();
            return Json(manager);
        }
    }
}
