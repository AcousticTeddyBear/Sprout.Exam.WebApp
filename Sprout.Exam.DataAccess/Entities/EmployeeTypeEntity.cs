using System.ComponentModel.DataAnnotations.Schema;

namespace Sprout.Exam.DataAccess.Entities
{
    [Table("EmployeeType")]
    public class EmployeeTypeEntity : BaseEntity
    {
        public string TypeName { get; set; }
    }
}
