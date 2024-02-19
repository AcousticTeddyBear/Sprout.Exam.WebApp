using NUnit.Framework;
using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Business.Models;
using Sprout.Exam.Common;

namespace Sprout.Exam.Tests.Business.Factories.SalaryFactory.Services
{
    [TestFixture]
    public class RegularSalaryServiceTests
    {
        BaseSalaryService salaryService;

        [SetUp]
        public void Setup()
        {
            salaryService = new RegularSalaryService();
        }

        [Test]
        public void CalculateSalary_ValidParams_ReturnsSalary()
        {
            var basicSalary = 20000m;
            var request = new CalculateSalaryRequest { AbsentDays = 1 };

            var result = salaryService.CalculateSalary(basicSalary, request);

            Assert.Multiple(() =>
            {
                var absentDeduction = request.AbsentDays * (basicSalary / Constants.WORK_MONTH_IN_DAYS);
                var taxDeduction = basicSalary * Constants.TAX_DEDUCTION_PERCENTAGE;
                Assert.AreEqual(basicSalary - absentDeduction - taxDeduction, result);
            });
        }
    }
}
