using NUnit.Framework;
using Sprout.Exam.Business.Factories.SalaryFactory;
using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Common.Exceptions;
using System.Collections.Generic;
using System.Net;

namespace Sprout.Exam.Tests.Business.Factories.SalaryFactory
{
    [TestFixture]
    public class SalaryServiceFactoryTests
    {
        List<BaseSalaryService> salaryServices;
        SalaryServiceFactory salaryServiceFactory;

        [SetUp]
        public void Setup()
        {
            salaryServices = new List<BaseSalaryService> { new RegularSalaryService(), new ContractualSalaryService() };
            salaryServiceFactory = new SalaryServiceFactory(salaryServices);
        }

        [Test]
        public void GetSalaryService_IsRegularEmployeeType_ReturnsRegularSalaryService()
        {
            var employeeType = EmployeeTypeEnum.Regular;

            var result = salaryServiceFactory.GetSalaryService(employeeType);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(typeof(RegularSalaryService), result.GetType());
                Assert.AreEqual(employeeType, result.EmployeeType);
            });
        }

        [Test]
        public void GetSalaryService_IsContractualEmployeeType_ReturnsContractualSalaryService()
        {
            var employeeType = EmployeeTypeEnum.Contractual;

            var result = salaryServiceFactory.GetSalaryService(employeeType);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(typeof(ContractualSalaryService), result.GetType());
                Assert.AreEqual(employeeType, result.EmployeeType);
            });
        }

        [Test]
        public void GetSalaryService_SalaryServiceDoesNotExist_ThrowsException()
        {
            var employeeType = EmployeeTypeEnum.Regular;
            salaryServiceFactory = new SalaryServiceFactory(new List<BaseSalaryService> { new ContractualSalaryService() });

            Assert.Multiple(() =>
            {
                var exception = Assert.Throws<BadRequestException>(() => salaryServiceFactory.GetSalaryService(employeeType));
                Assert.AreEqual(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.AreEqual("Invalid Employee Type", exception.Message);
            });
        }
    }
}
