using Management.Data;
using Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    public class DepartmentController:Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departments =await _context.Departments.ToListAsync();
            return View(departments);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]DepartmentRequest request)
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
                var existingDepts =await _context.Departments.Select(d => d.Name.ToLower()).ToListAsync() ?? new List<string>();
                Console.WriteLine(existingDepts.Count);
                var dupliNames = request.name.Where(name => existingDepts.Contains(name.ToLower())).ToList() ?? new List<string>();

                if (dupliNames.Any())
                {
                    return Json(new { success = false, errors = dupliNames });
                }
                var newDepts=request.name.Select(name => new Department { Name=name}).ToList();
                _context.Departments.AddRange(newDepts);
                _context.SaveChangesAsync();

                return Json(new { success = true,message="Departments added successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        [HttpPost]        
        public async Task<IActionResult> Edit([FromBody]Department department)
        {
            if (department == null || string.IsNullOrWhiteSpace(department.Name))
            {
                return Json(new { success = false, message = "Invalid data" });
            }
            var existing=await _context.Departments.FirstOrDefaultAsync(d=>d.Id==department.Id);
            if (existing == null)
            {
                return Json(new { success = false, message = "Department not found" });
            }
            bool isDuplicate=await _context.Departments.AnyAsync(d=>d.Name.ToLower()==department.Name.ToLower() && d.Id!=department.Id);
            if (isDuplicate) {
                return Json(new { success = false, message = "Department already exists!!" });
            }
            existing.Name=department.Name;
            _context.SaveChangesAsync();
            return Json(new { success = true, message = "Department Updated successfully" });
        }


        [HttpGet]
        public async  Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                Console.WriteLine(id);
                var department =await _context.Departments.FirstOrDefaultAsync(dept => dept.Id == id);
                if (department == null)
                {
                    return Json(new { success = false, message = "Department not found" });
                }
                var employeesInDept =await _context.Employees.AnyAsync(e => e.DepartmentId == id);
                if (employeesInDept) {
                    return Json(new { success = false, message = "Cannot Delete Department. There are some employees assigned to this Department" });
                }
                _context.Departments.Remove(department);
                _context.SaveChangesAsync();
                return Json(new { success = true, message = "Department Deleted Successfully!" });
            }
            catch (Exception ex) { 
                return Json(new {success=false, message=ex.Message});
            }
        }
    }
}

public class DepartmentRequest
{
    public List<string> name { get; set; }
}