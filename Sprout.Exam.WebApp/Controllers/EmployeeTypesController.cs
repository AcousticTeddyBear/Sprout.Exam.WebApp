using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprout.Exam.Business.Services;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/employee-types")]
    [ApiController]
    public class EmployeeTypesController : ControllerBase
    {
        private readonly IEmployeeTypesService employeeTypesService;

        public EmployeeTypesController(IEmployeeTypesService employeeTypesService)
        {
            this.employeeTypesService = employeeTypesService;
        }

        /// <summary>
        /// Get all employee types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await employeeTypesService.GetEmployeeTypes();
            return Ok(result);
        }
    }
}
