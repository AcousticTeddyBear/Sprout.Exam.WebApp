using Microsoft.EntityFrameworkCore;
using Sprout.Exam.DataAccess.Entities;

namespace Sprout.Exam.DataAccess
{
    public class SproutExamDbContext : DbContext
    {
        public SproutExamDbContext(DbContextOptions<SproutExamDbContext> options)
        : base(options)
        {
        }

        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<EmployeeTypeEntity> EmployeeTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EmployeeEntity>().HasKey(x => x.Id);
            builder.Entity<EmployeeTypeEntity>().HasKey(x => x.Id);
        }
    }
}

