using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Sprout.Exam.Business.Factories.SalaryFactory
{
    public class SalaryServiceFactory : ISalaryServiceFactory
    {
        private readonly IEnumerable<BaseSalaryService> employees;

        public SalaryServiceFactory(IEnumerable<BaseSalaryService> employees) => this.employees = employees;

        public BaseSalaryService GetEmployee(EmployeeTypeEnum employeeType)
            => employees.SingleOrDefault(e => e.EmployeeType == employeeType) ?? throw new BadRequestException("Invalid Employee Type");
    }
}
