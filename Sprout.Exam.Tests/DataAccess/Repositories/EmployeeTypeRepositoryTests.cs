using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
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
    public class EmployeeTypeRepositoryTests
    {
        List<EmployeeTypeEntity> EMPLOYEE_TYPE_LIST = new()
        {
            new() {
                Id = 1,
                TypeName = "Regular"
            },
            new() {
                Id = 2,
                TypeName = "Contractual"
            }
        };

        SproutExamDbContext dbContext;
        EmployeeTypeRepository employeeTypeRepository;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<SproutExamDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            dbContext = new SproutExamDbContext(options);
            dbContext.AddRange(EMPLOYEE_TYPE_LIST);
            dbContext.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            employeeTypeRepository = new EmployeeTypeRepository(dbContext);
        }

        [Test]
        public async Task Get_ReturnsExpected()
        {
            var result = await employeeTypeRepository.Get().ToListAsync();

            Assert.AreEqual(dbContext.EmployeeTypes, result);
        }

        [Test]
        public async Task Get_WithPredicate_ReturnsExpected()
        {
            var result = await employeeTypeRepository.Get(e => e.Id == 1).ToListAsync();

            Assert.AreEqual(dbContext.EmployeeTypes.Where(e => e.Id == 1).ToList(), result);
        }

        [Test]
        public async Task Single_Exists_ReturnsExpected()
        {
            var result = await employeeTypeRepository.Single(x => x.Id == EMPLOYEE_TYPE_LIST[0].Id);

            Assert.AreEqual(EMPLOYEE_TYPE_LIST[0], result);
        }

        [Test]
        public async Task Single_DoesNotExist_ReturnsDefault()
        {
            var result = await employeeTypeRepository.Single(x => x.Id == 9999);

            Assert.AreEqual(default(EmployeeEntity), result);
        }

        [Test]
        public async Task Add_ReturnsExpected()
        {
            var newEmployeeType = new EmployeeTypeEntity
            {
                TypeName = "Probationary"
            };

            var result = await employeeTypeRepository.Add(newEmployeeType);

            Assert.AreEqual(newEmployeeType, result);
        }

        [Test]
        public async Task Update_ReturnsExpected()
        {
            var updatedEmployeeType = new EmployeeTypeEntity
            {
                TypeName = "Part-Time"
            };


            var result = await employeeTypeRepository.Update(updatedEmployeeType);

            Assert.AreEqual(updatedEmployeeType, result);
        }

        [Test]
        public async Task Delete_ReturnsExpected()
        {
            var employeeType = new EmployeeTypeEntity
            {
                TypeName = "Contractual"
            };

            var employeeTypeToDelete = dbContext.EmployeeTypes.Add(employeeType).Entity;
            await dbContext.SaveChangesAsync();

            await employeeTypeRepository.Delete(employeeTypeToDelete);

            Assert.IsFalse(dbContext.EmployeeTypes.Contains(employeeTypeToDelete));
        }
    }
}
