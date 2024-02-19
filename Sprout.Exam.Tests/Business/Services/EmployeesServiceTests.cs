using AutoMapper;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Sprout.Exam.Business.Factories.SalaryFactory;
using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Business.Mapping;
using Sprout.Exam.Business.Models;
using Sprout.Exam.Business.Services;
using Sprout.Exam.Common;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Common.Exceptions;
using Sprout.Exam.DataAccess.Entities;
using Sprout.Exam.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Sprout.Exam.Tests.Business.Services
{
    [TestFixture]
    public class EmployeesServiceTests
    {
        Mock<ISalaryServiceFactory> mockSalaryServiceFactory;
        Mock<IEmployeeRepository> mockEmployeeRepository;
        IMapper mapper;
        EmployeesService employeesService;

        [SetUp]
        public void Setup()
        {
            mockSalaryServiceFactory = new Mock<ISalaryServiceFactory>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();

            var mappingProfile = new SproutExamMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            mapper = new Mapper(configuration);

            employeesService = new EmployeesService(mockSalaryServiceFactory.Object, mockEmployeeRepository.Object, mapper);
        }

        [Test]
        public async Task GetEmployees_HasData_ReturnsEmployees()
        {
            var employee1 = new EmployeeEntity
            {
                Id = 1,
                FullName = "Jane Doe",
                Birthdate = DateTime.Now,
                Tin = "123215413",
                BasicSalary = 20000,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                IsDeleted = false
            };

            var employee2 = new EmployeeEntity
            {
                Id = 2,
                FullName = "John Smith",
                Birthdate = DateTime.Now,
                Tin = "957125412",
                BasicSalary = 500,
                EmployeeTypeId = EmployeeTypeEnum.Contractual,
                IsDeleted = false
            };

            var employees = new List<EmployeeEntity>
            {
                employee1, employee2
            };

            mockEmployeeRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .Returns(employees.BuildMock());

            var result = await employeesService.GetEmployees();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.IsNotEmpty(result);
                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Exists(e => e.Id == employee1.Id
                && e.FullName == employee1.FullName
                && e.Birthdate == employee1.Birthdate.ToString(Constants.BIRTHDATE_FORMAT)
                && e.Tin == employee1.Tin
                && e.BasicSalary == employee1.BasicSalary
                && e.TypeId == (int)employee1.EmployeeTypeId));

                Assert.IsTrue(result.Exists(e => e.Id == employee2.Id
                && e.FullName == employee2.FullName
                && e.Birthdate == employee2.Birthdate.ToString(Constants.BIRTHDATE_FORMAT)
                && e.Tin == employee2.Tin
                && e.BasicSalary == employee2.BasicSalary
                && e.TypeId == (int)employee2.EmployeeTypeId));
            });
        }

        [Test]
        public async Task GetEmployeeById_EmployeeExists_ReturnsEmployee()
        {
            var employee1 = new EmployeeEntity
            {
                Id = 1,
                FullName = "Jane Doe",
                Birthdate = DateTime.Now,
                Tin = "123215413",
                BasicSalary = 20000,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                IsDeleted = false
            };

            mockEmployeeRepository
                .Setup(x => x.Single(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .ReturnsAsync(employee1);

            var result = await employeesService.GetEmployeeById(employee1.Id);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(employee1.Id, result.Id);
                Assert.AreEqual(employee1.FullName, result.FullName);
                Assert.AreEqual(employee1.Birthdate.ToString(Constants.BIRTHDATE_FORMAT), result.Birthdate);
                Assert.AreEqual(employee1.Tin, result.Tin);
                Assert.AreEqual(employee1.BasicSalary, result.BasicSalary);
                Assert.AreEqual((int)employee1.EmployeeTypeId, result.TypeId);
            });
        }

        [Test]
        public async Task GetEmployeeById_EmployeeDoesNotExist_ThrowsException()
        {
            mockEmployeeRepository
                .Setup(x => x.Single(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .ReturnsAsync(default(EmployeeEntity));

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<NotFoundException>(() => employeesService.GetEmployeeById(999));
                Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);
                Assert.AreEqual("Employee does not exist.", exception.Message);
            });
        }

        [Test]
        public async Task CreateEmployee_ValidParams_ReturnsEmployee()
        {
            var employeeRequest = new EmployeeRequest
            {
                FullName = "Jane Doe",
                Birthdate = DateTime.Now,
                Tin = "123215413",
                BasicSalary = 20000,
                TypeId = EmployeeTypeEnum.Regular
            };

            mockEmployeeRepository
                .Setup(x => x.Add(It.IsAny<EmployeeEntity>()))
                .ReturnsAsync(mapper.Map<EmployeeEntity>(employeeRequest, opts => opts.AfterMap((src, dest) => dest.Id = 1)));

            var result = await employeesService.CreateEmployee(employeeRequest);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(employeeRequest.FullName, result.FullName);
                Assert.AreEqual(employeeRequest.Birthdate?.ToString(Constants.BIRTHDATE_FORMAT), result.Birthdate);
                Assert.AreEqual(employeeRequest.Tin, result.Tin);
                Assert.AreEqual(employeeRequest.BasicSalary, result.BasicSalary);
                Assert.AreEqual((int)employeeRequest.TypeId, result.TypeId);
            });
        }

        [Test]
        public async Task UpdateEmployee_ValidParams_ReturnsUpdatedEmployee()
        {
            var employeeId = 1;
            var employeeRequest = new EmployeeRequest
            {
                FullName = "Jane Doe New",
                Birthdate = DateTime.Now.AddMonths(-60),
                Tin = "543217654",
                BasicSalary = 25000,
                TypeId = EmployeeTypeEnum.Regular
            };

            var employeeEntity = new EmployeeEntity
            {
                Id = employeeId,
                FullName = "Jane Doe",
                Birthdate = DateTime.Now,
                Tin = "123215413",
                BasicSalary = 20000,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                IsDeleted = false
            };

            mockEmployeeRepository
                .Setup(x => x.Single(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .ReturnsAsync(employeeEntity);

            mockEmployeeRepository
                .Setup(x => x.Update(mapper.Map(employeeRequest, employeeEntity)))
                .ReturnsAsync(employeeEntity);

            var result = await employeesService.UpdateEmployee(employeeId, employeeRequest);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(employeeId, result.Id);
                Assert.AreEqual(employeeRequest.FullName, result.FullName);
                Assert.AreEqual(employeeRequest.Birthdate?.ToString(Constants.BIRTHDATE_FORMAT), result.Birthdate);
                Assert.AreEqual(employeeRequest.Tin, result.Tin);
                Assert.AreEqual(employeeRequest.BasicSalary, result.BasicSalary);
                Assert.AreEqual((int)employeeRequest.TypeId, result.TypeId);
            });
        }

        [Test]
        public async Task UpdateEmployee_EmployeeDoesNotExist_ThrowsException()
        {
            var employeeId = 1;
            var employeeRequest = new EmployeeRequest
            {
                FullName = "Jane Doe New",
                Birthdate = DateTime.Now.AddMonths(-60),
                Tin = "543217654",
                BasicSalary = 25000,
                TypeId = EmployeeTypeEnum.Regular
            };

            mockEmployeeRepository
                .Setup(x => x.Single(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .ReturnsAsync(default(EmployeeEntity));

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<NotFoundException>(() => employeesService.UpdateEmployee(employeeId, employeeRequest));
                Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);
                Assert.AreEqual("Employee does not exist.", exception.Message);
            });
        }

        [Test]
        public async Task DeleteEmployee_ValidParams_DeletesEmployee()
        {
            var employeeId = 1;
            var employeeEntity = new EmployeeEntity
            {
                Id = employeeId,
                FullName = "Jane Doe",
                Birthdate = DateTime.Now,
                Tin = "123215413",
                BasicSalary = 20000,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                IsDeleted = false
            };

            mockEmployeeRepository
                .Setup(x => x.Single(e => e.Id == employeeId && !e.IsDeleted))
                .ReturnsAsync(employeeEntity);

            employeeEntity.IsDeleted = true;

            mockEmployeeRepository
                .Setup(x => x.Update(employeeEntity))
                .ReturnsAsync(employeeEntity);

            await employeesService.DeleteEmployee(employeeId);

            Assert.Multiple(() =>
            {
                mockEmployeeRepository.Verify(x => x.Single(e => e.Id == employeeId && !e.IsDeleted));
                mockEmployeeRepository.Verify(x => x.Update(employeeEntity));
            });
        }

        [Test]
        public async Task DeleteEmployee_EmployeeDoesNotExist_ThrowsException()
        {
            var employeeId = 1;

            mockEmployeeRepository
                .Setup(x => x.Single(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .ReturnsAsync(default(EmployeeEntity));

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<NotFoundException>(() => employeesService.DeleteEmployee(employeeId));
                Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);
                Assert.AreEqual("Employee does not exist.", exception.Message);
            });
        }

        [Test]
        public async Task CalculateSalary_IsRegularEmployee_ReturnsNetSalary()
        {
            var employee1 = new EmployeeEntity
            {
                Id = 1,
                FullName = "Jane Doe",
                Birthdate = DateTime.Now,
                Tin = "123215413",
                BasicSalary = 20000,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                IsDeleted = false
            };

            var calculateSalaryRequest = new CalculateSalaryRequest
            {
                AbsentDays = 1,
            };

            mockEmployeeRepository
                .Setup(x => x.Single(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .ReturnsAsync(employee1);

            mockSalaryServiceFactory
                .Setup(x => x.GetSalaryService(employee1.EmployeeTypeId))
                .Returns(new RegularSalaryService());

            var result = await employeesService.CalculateSalary(employee1.Id, calculateSalaryRequest);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);

                var absentDeduction = calculateSalaryRequest.AbsentDays * (employee1.BasicSalary / Constants.WORK_MONTH_IN_DAYS);
                var taxDeduction = employee1.BasicSalary * Constants.TAX_DEDUCTION_PERCENTAGE;
                var expected = Math.Round(employee1.BasicSalary - absentDeduction - taxDeduction, 2);
                Assert.AreEqual(expected, result.Salary);
            });
        }

        [Test]
        public async Task CalculateSalary_IsContractualEmployee_ReturnsNetSalary()
        {
            var employee2 = new EmployeeEntity
            {
                Id = 2,
                FullName = "John Smith",
                Birthdate = DateTime.Now,
                Tin = "957125412",
                BasicSalary = 500,
                EmployeeTypeId = EmployeeTypeEnum.Contractual,
                IsDeleted = false
            };

            var calculateSalaryRequest = new CalculateSalaryRequest
            {
                WorkedDays = 15.5m
            };

            mockEmployeeRepository
                .Setup(x => x.Single(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .ReturnsAsync(employee2);

            mockSalaryServiceFactory
                .Setup(x => x.GetSalaryService(employee2.EmployeeTypeId))
                .Returns(new ContractualSalaryService());

            var result = await employeesService.CalculateSalary(employee2.Id, calculateSalaryRequest);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                var expected = Math.Round(employee2.BasicSalary * calculateSalaryRequest.WorkedDays, 2);
                Assert.AreEqual(expected, result.Salary);
            });
        }

        [Test]
        public async Task CalculateSalary_EmployeeDoesNotExist_ThrowsException()
        {
            mockEmployeeRepository
                .Setup(x => x.Single(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                .ReturnsAsync(default(EmployeeEntity));

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<NotFoundException>(() => employeesService.CalculateSalary(999, new CalculateSalaryRequest()));
                Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);
                Assert.AreEqual("Employee does not exist.", exception.Message);
            });
        }
    }
}
