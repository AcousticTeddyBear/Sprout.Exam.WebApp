using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.DataAccess;
using Sprout.Exam.DataAccess.Entities;
using Sprout.Exam.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.Tests.DataAccess.Repositories
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        List<EmployeeEntity> EMPLOYEE_LIST = new()
        {
            new() {
                Id = 1,
                FullName = "Jane Doe",
                Birthdate = DateTime.Now,
                Tin = "123215413",
                BasicSalary = 20000,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                IsDeleted = false
            },
            new() {
                Id = 2,
                FullName = "John Smith",
                Birthdate = DateTime.Now,
                Tin = "957125412",
                BasicSalary = 500,
                EmployeeTypeId = EmployeeTypeEnum.Contractual,
                IsDeleted = false
            }
        };

        SproutExamDbContext dbContext;
        EmployeeRepository employeeRepository;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<SproutExamDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            dbContext = new SproutExamDbContext(options);
            dbContext.AddRange(EMPLOYEE_LIST);
            dbContext.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            employeeRepository = new EmployeeRepository(dbContext);
        }

        [Test]
        public async Task Get_ReturnsExpected()
        {
            var result = await employeeRepository.Get().ToListAsync();

            Assert.AreEqual(dbContext.Employees, result);
        }

        [Test]
        public async Task Get_WithPredicate_ReturnsExpected()
        {
            var result = await employeeRepository.Get(e => e.Id == 1).ToListAsync();

            Assert.AreEqual(dbContext.Employees.Where(e => e.Id == 1).ToList(), result);
        }

        [Test]
        public async Task Single_Exists_ReturnsExpected()
        {
            var result = await employeeRepository.Single(x => x.Id == EMPLOYEE_LIST[0].Id);

            Assert.AreEqual(EMPLOYEE_LIST[0], result);
        }

        [Test]
        public async Task Single_DoesNotExist_ReturnsDefault()
        {
            var result = await employeeRepository.Single(x => x.Id == 9999);

            Assert.AreEqual(default(EmployeeEntity), result);
        }

        [Test]
        public async Task Add_ReturnsExpected()
        {
            var newEmployee = new EmployeeEntity
            {
                FullName = "Test",
                BasicSalary = 150000,
                Birthdate = DateTime.Now,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                Tin = "123456789"
            };

            var result = await employeeRepository.Add(newEmployee);

            Assert.AreEqual(newEmployee, result);
        }

        [Test]
        public async Task Update_ReturnsExpected()
        {
            var updatedEmployee = new EmployeeEntity
            {
                FullName = "Test",
                BasicSalary = 150000,
                Birthdate = DateTime.Now,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                Tin = "123456789"
            };

            var result = await employeeRepository.Update(updatedEmployee);

            Assert.AreEqual(updatedEmployee, result);
        }

        [Test]
        public async Task Delete_ReturnsExpected()
        {
            var employee = new EmployeeEntity
            {
                FullName = "Test",
                BasicSalary = 150000,
                Birthdate = DateTime.Now,
                EmployeeTypeId = EmployeeTypeEnum.Regular,
                Tin = "123456789"
            };

            var employeeToDelete = dbContext.Employees.Add(employee).Entity;
            await dbContext.SaveChangesAsync();

            await employeeRepository.Delete(employeeToDelete);

            Assert.IsFalse(dbContext.Employees.Contains(employeeToDelete));
        }
    }
}
