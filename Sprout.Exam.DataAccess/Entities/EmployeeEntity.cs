using Sprout.Exam.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprout.Exam.DataAccess.Entities
{
    [Table("Employee")]
    public class EmployeeEntity : BaseEntity
    {
        public string FullName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Tin { get; set; }
        public EmployeeTypeEnum EmployeeTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public decimal BasicSalary { get; set; }
    }
}
