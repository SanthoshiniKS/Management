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

        public IActionResult Index()
        {
            var departments =_context.Departments.ToList();
            return View(departments);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromBody]DepartmentRequest request)
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
                var existingDepts = _context.Departments.Select(d => d.Name.ToLower()).ToList() ?? new List<string>();
                Console.WriteLine(existingDepts.Count);
                var dupliNames = request.name.Where(name => existingDepts.Contains(name.ToLower())).ToList() ?? new List<string>();

                if (dupliNames.Any())
                {
                    return Json(new { success = false, errors = dupliNames });
                }
                var newDepts=request.name.Select(name => new Department { Name=name}).ToList();
                _context.Departments.AddRange(newDepts);
                _context.SaveChanges();

                return Json(new { success = true,message="Departments added successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        [HttpPost]        
        public IActionResult Edit([FromBody]Department department)
        {
            if (department == null || string.IsNullOrWhiteSpace(department.Name))
            {
                return Json(new { success = false, message = "Invalid data" });
            }
            var existing=_context.Departments.FirstOrDefault(d=>d.Id==department.Id);
            if (existing == null)
            {
                return Json(new { success = false, message = "Department not found" });
            }
            bool isDuplicate=_context.Departments.Any(d=>d.Name.ToLower()==department.Name.ToLower() && d.Id!=department.Id);
            if (isDuplicate) {
                return Json(new { success = false, message = "Department already exists!!" });
            }
            existing.Name=department.Name;
            _context.SaveChanges();
            return Json(new { success = true, message = "Department Updated successfully" });
        }


        [HttpGet]
        public IActionResult DeleteDepartment(int id)
        {
            try
            {
                Console.WriteLine(id);
                var department = _context.Departments.FirstOrDefault(dept => dept.Id == id);
                if (department == null)
                {
                    return Json(new { success = false, message = "Department not found" });
                }
                var employeesInDept = _context.Employees.Any(e => e.DepartmentId == id);
                if (employeesInDept) {
                    return Json(new { success = false, message = "Cannot Delete Department. There are some employees assigned to this Department" });
                }
                _context.Departments.Remove(department);
                _context.SaveChanges();
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