using NUnit.Framework;
using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Business.Models;

namespace Sprout.Exam.Tests.Business.Factories.SalaryFactory.Services
{
    [TestFixture]
    public class ContractualSalaryServiceTests
    {
        BaseSalaryService salaryService;

        [SetUp]
        public void Setup()
        {
            salaryService = new ContractualSalaryService();
        }

        [Test]
        public void CalculateSalary_ValidParams_ReturnsSalary()
        {
            var basicSalary = 500m;
            var request = new CalculateSalaryRequest { WorkedDays = 15.5m };

            var result = salaryService.CalculateSalary(basicSalary, request);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(basicSalary * request.WorkedDays, result);
            });
        }
    }
}
