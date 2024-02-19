using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprout.Exam.Business.Models;
using Sprout.Exam.Business.Services;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await employeesService.GetEmployees();
            return Ok(result);
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await employeesService.GetEmployeeById(id);
            return Ok(result);
        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, EmployeeRequest input)
        {
            var item = await employeesService.UpdateEmployee(id, input);
            return Ok(item);
        }

        /// <summary>
        /// Create employee
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(EmployeeRequest input)
        {
            var employee = await employeesService.CreateEmployee(input);

            return Created($"/api/employees/{employee.Id}", employee.Id);
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await employeesService.DeleteEmployee(id);
            return NoContent();
        }

        /// <summary>
        /// Calculate employee salary
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate([FromRoute] int id, CalculateSalaryRequest calculateSalaryRequest)
        {
            var response = await employeesService.CalculateSalary(id, calculateSalaryRequest);
            return Ok(response);
        }

    }
}
