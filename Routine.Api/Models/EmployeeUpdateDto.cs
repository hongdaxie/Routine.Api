using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Routine.Api.Entities;
using Routine.Api.ValidationAttributes;

namespace Routine.Api.Models
{
    public class EmployeeUpdateDto
    {
        [Display(Name = "employee id")]
        [Required(ErrorMessage = "{0} is required")]
        public string EmployeeNo { get; set; }
        [Display(Name = "first name")]
        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(50, ErrorMessage = "{0}`s length cannot more than {1}")]
        public string FirstName { get; set; }
        [Display(Name = "last name"), Required(ErrorMessage = "{0 is required}"), MaxLength(50, ErrorMessage = "{0}`s length cannot more than {1}")]
        public string LastName { get; set; }
        [Display(Name = "gender")]
        public Gender Gender { get; set; }
        [Display(Name = "date of birth")]
        public DateTime DateOfBirth { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                yield return new ValidationResult("First name and last name cannot be same.",
                    new[] { nameof(FirstName), nameof(LastName) });
            }
        }
    }
}
