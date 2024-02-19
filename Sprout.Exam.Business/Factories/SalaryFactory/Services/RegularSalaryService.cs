using Sprout.Exam.Business.Models;
using Sprout.Exam.Common;
using Sprout.Exam.Common.Enums;

namespace Sprout.Exam.Business.Factories.SalaryFactory.Services
{
    public class RegularSalaryService : BaseSalaryService
    {
        public RegularSalaryService() : base(EmployeeTypeEnum.Regular) { }

        public override decimal CalculateSalary(decimal salary, CalculateSalaryRequest calculateSalaryRequest)
        {
            var absentDeduction = calculateSalaryRequest.AbsentDays * (salary / Constants.WORK_MONTH_IN_DAYS);
            var taxDeduction = salary * Constants.TAX_DEDUCTION_PERCENTAGE;
            return salary - absentDeduction - taxDeduction;
        }
    }
}
