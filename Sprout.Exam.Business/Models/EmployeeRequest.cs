using Sprout.Exam.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sprout.Exam.Business.Models
{
    public class EmployeeRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Tin { get; set; }

        [Required]
        public DateTime? Birthdate { get; set; }

        [Required]
        [EnumDataType(typeof(EmployeeTypeEnum))]
        public EmployeeTypeEnum? TypeId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public decimal BasicSalary { get; set; }
    }
}
