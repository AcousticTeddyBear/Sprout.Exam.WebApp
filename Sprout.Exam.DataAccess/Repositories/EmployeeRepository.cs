using Sprout.Exam.DataAccess.Entities;

namespace Sprout.Exam.DataAccess.Repositories
{
    public class EmployeeRepository : BaseRepository<EmployeeEntity>, IEmployeeRepository
    {
        public EmployeeRepository(SproutExamDbContext dbContext) : base(dbContext)
        {
        }
    }
}
