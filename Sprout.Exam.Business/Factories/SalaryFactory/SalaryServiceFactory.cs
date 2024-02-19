using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Sprout.Exam.Business.Factories.SalaryFactory
{
    public class SalaryServiceFactory : ISalaryServiceFactory
    {
        private readonly IEnumerable<BaseSalaryService> salaryServices;

        public SalaryServiceFactory(IEnumerable<BaseSalaryService> salaryServices) => this.salaryServices = salaryServices;

        public BaseSalaryService GetSalaryService(EmployeeTypeEnum employeeType)
            => salaryServices.SingleOrDefault(e => e.EmployeeType == employeeType) ?? throw new BadRequestException("Invalid Employee Type");
    }
}
