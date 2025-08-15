using System.ComponentModel.DataAnnotations;
namespace EmployeeMgmtMvc.Models
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        public MinAgeAttribute(int minAge)=>_minAge=minAge;
        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is DateTime dob)
            {
                var age = DateTime.Today.Year - dob.Year;
                if (dob.Date > DateTime.Today.AddYears(-age)) age--;
                return age>=_minAge ? ValidationResult.Success : new ValidationResult($"Usia minimal {_minAge} tahun.");
            }
            return ValidationResult.Success;
        }
    }
}