using Sprout.Exam.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprout.Exam.Business.Services
{
    public interface IEmployeeTypesService
    {
        Task<List<EmployeeTypeEntity>> GetEmployeeTypes();
    }
}
