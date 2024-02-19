using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.Factories.SalaryFactory;
using Sprout.Exam.Business.Models;
using Sprout.Exam.Common.Exceptions;
using Sprout.Exam.DataAccess.Entities;
using Sprout.Exam.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprout.Exam.Business.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly ISalaryServiceFactory salaryServiceFactory;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        public EmployeesService(ISalaryServiceFactory salaryServiceFactory, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            this.salaryServiceFactory = salaryServiceFactory;
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }

        public async Task<CalculateSalaryResponse> CalculateSalary(int id, CalculateSalaryRequest calculateSalaryRequest)
        {
            var employee = await getEmployeeEntity(id);

            var salaryService = salaryServiceFactory.GetSalaryService(employee.EmployeeTypeId);
            var netSalary = salaryService.CalculateSalary(employee.BasicSalary, calculateSalaryRequest);

            return new CalculateSalaryResponse() { Salary = Math.Round(netSalary, 2) };
        }

        public async Task<EmployeeDto> CreateEmployee(EmployeeRequest employeeRequest)
        {
            var employeeEntity = mapper.Map<EmployeeEntity>(employeeRequest);
            var createdEmployee = await employeeRepository.Add(employeeEntity);
            return mapper.Map<EmployeeDto>(createdEmployee);
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await getEmployeeEntity(id);
            employee.IsDeleted = true;
            await employeeRepository.Update(employee);
        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            var employee = await getEmployeeEntity(id);
            return mapper.Map<EmployeeDto>(employee);
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            var employees = await employeeRepository.Get(e => !e.IsDeleted).ToListAsync();
            return mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> UpdateEmployee(int id, EmployeeRequest employeeRequest)
        {
            var employee = await getEmployeeEntity(id);

            mapper.Map(employeeRequest, employee);

            var updatedEmployee = await employeeRepository.Update(employee);
            return mapper.Map<EmployeeDto>(updatedEmployee);
        }

        private async Task<EmployeeEntity> getEmployeeEntity(int id)
        {
            return await employeeRepository.Single(e => e.Id == id && !e.IsDeleted)
                ?? throw new NotFoundException("Employee does not exist.");
        }
    }
}
