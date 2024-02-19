using Sprout.Exam.DataAccess.Entities;

namespace Sprout.Exam.DataAccess.Repositories
{
    public class EmployeeTypeRepository : BaseRepository<EmployeeTypeEntity>, IEmployeeTypeRepository
    {
        public EmployeeTypeRepository(SproutExamDbContext dbContext) : base(dbContext)
        {
        }
    }
}
