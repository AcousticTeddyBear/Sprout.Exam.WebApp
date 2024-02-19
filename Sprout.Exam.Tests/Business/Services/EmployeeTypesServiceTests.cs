using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Sprout.Exam.Business.Services;
using Sprout.Exam.DataAccess.Entities;
using Sprout.Exam.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sprout.Exam.Tests.Business.Services
{
    [TestFixture]
    public class EmployeeTypesServiceTests
    {
        Mock<IEmployeeTypeRepository> mockEmployeeTypeRepository;
        EmployeeTypesService employeeTypesService;

        [SetUp]
        public void Setup()
        {
            mockEmployeeTypeRepository = new Mock<IEmployeeTypeRepository>();
            employeeTypesService = new EmployeeTypesService(mockEmployeeTypeRepository.Object);
        }

        [Test]
        public async Task GetEmployeeTypes_HasData_ReturnsEmployeeTypes()
        {
            var employeeType1 = new EmployeeTypeEntity
            {
                Id = 1,
                TypeName = "Regular"
            };

            var employeeType2 = new EmployeeTypeEntity
            {
                Id = 2,
                TypeName = "Contractual"
            };

            var employeeTypes = new List<EmployeeTypeEntity>
            {
                employeeType1,
                employeeType2
            };

            mockEmployeeTypeRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<EmployeeTypeEntity, bool>>>()))
                .Returns(employeeTypes.BuildMock());

            var result = await employeeTypesService.GetEmployeeTypes();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.IsNotEmpty(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(employeeTypes, result);
            });
        }
    }
}
