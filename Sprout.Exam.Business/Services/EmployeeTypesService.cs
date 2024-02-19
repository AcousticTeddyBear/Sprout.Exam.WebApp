using Microsoft.EntityFrameworkCore;
using Sprout.Exam.DataAccess.Entities;
using Sprout.Exam.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprout.Exam.Business.Services
{
    public class EmployeeTypesService : IEmployeeTypesService
    {
        private readonly IEmployeeTypeRepository employeeTypeRepository;

        public EmployeeTypesService(IEmployeeTypeRepository employeeTypeRepository) => this.employeeTypeRepository = employeeTypeRepository;

        public Task<List<EmployeeTypeEntity>> GetEmployeeTypes() => employeeTypeRepository.Get().ToListAsync();
    }
}
