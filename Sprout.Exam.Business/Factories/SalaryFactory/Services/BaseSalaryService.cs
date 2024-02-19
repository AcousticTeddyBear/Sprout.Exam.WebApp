using Sprout.Exam.Business.Models;
using Sprout.Exam.Common.Enums;

namespace Sprout.Exam.Business.Factories.SalaryFactory.Services
{
    public abstract class BaseSalaryService
    {
        public EmployeeTypeEnum EmployeeType { get; }

        protected BaseSalaryService(EmployeeTypeEnum employeeType) => EmployeeType = employeeType;

        public abstract decimal CalculateSalary(decimal salary, CalculateSalaryRequest calculateSalaryRequest);
    }
}
