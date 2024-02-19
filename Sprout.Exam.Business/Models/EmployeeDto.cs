namespace Sprout.Exam.Business.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Birthdate { get; set; }
        public string Tin { get; set; }
        public int TypeId { get; set; }
        public decimal BasicSalary { get; set; }
    }
}
