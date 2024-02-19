using Sprout.Exam.Business.Models;
using Sprout.Exam.Common.Enums;

namespace Sprout.Exam.Business.Factories.SalaryFactory.Services
{
    public class ContractualSalaryService : BaseSalaryService
    {
        public ContractualSalaryService() : base(EmployeeTypeEnum.Contractual) { }

        public override decimal CalculateSalary(decimal salary, CalculateSalaryRequest calculateSalaryRequest)
            => salary * calculateSalaryRequest.WorkedDays;
    }
}
