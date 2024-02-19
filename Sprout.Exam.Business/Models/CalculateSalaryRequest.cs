using System.ComponentModel.DataAnnotations;

namespace Sprout.Exam.Business.Models
{
    public class CalculateSalaryRequest
    {
        [Range(0, int.MaxValue)]
        public decimal AbsentDays { get; set; }


        [Range(0, int.MaxValue)]
        public decimal WorkedDays { get; set; }
    }
}
