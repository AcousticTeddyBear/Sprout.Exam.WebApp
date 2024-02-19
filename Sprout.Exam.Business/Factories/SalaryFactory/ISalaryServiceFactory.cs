using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Common.Enums;

namespace Sprout.Exam.Business.Factories.SalaryFactory
{
    public interface ISalaryServiceFactory
    {
        BaseSalaryService GetSalaryService(EmployeeTypeEnum employeeType);
    }
}
