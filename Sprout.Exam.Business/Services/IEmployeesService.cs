using Sprout.Exam.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprout.Exam.Business.Services
{
    public interface IEmployeesService
    {
        Task<List<EmployeeDto>> GetEmployees();
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<EmployeeDto> CreateEmployee(EmployeeRequest employeeRequest);
        Task<EmployeeDto> UpdateEmployee(int id, EmployeeRequest employeeRequest);
        Task DeleteEmployee(int id);
        Task<CalculateSalaryResponse> CalculateSalary(int id, CalculateSalaryRequest calculateSalaryRequest);
    }
}
