using Management.Data;
using Management.Models;

namespace Management.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;
        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Employee GetEmployeeById(int id) { 
            return _context.Employees.FirstOrDefault(e => e.Id == id);
        }

        public bool DeleteEmployee(int id)
        {
            var employee= _context.Employees.FirstOrDefault(e=>e.Id == id);
            if (employee==null)
            {
                return false;
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return true;
        }

        public void UpdateEmployeesManager(int deptId,int managerId)
        {
            var empInDept=_context.Employees
                .Where(e=>e.DepartmentId == deptId && e.ManagerID==managerId)
                .ToList();
            foreach (var emp in empInDept) {
                emp.ManagerID = null;
            }
            _context.SaveChanges();
        }

        public bool UpdateEmployee(Employee updatedEmployee)
        {
            var employee=_context.Employees.FirstOrDefault(e=>e.Id==updatedEmployee.Id);
            if (employee == null) return false;
            employee.Name=updatedEmployee.Name;
            employee.DepartmentId= updatedEmployee.DepartmentId;
            employee.ManagerID = updatedEmployee.ManagerID;
            _context.SaveChanges();
            return true;
        }
    }
}
