using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    public class CompanyAddDto
    {
        [Display(Name = "Company name")]
        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(100, ErrorMessage = "{0}`s max length cannot over {1}")]
        public string Name { get; set; }
        [Display(Name = "Company description")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "{0}`s length should between {2} to {1}")]
        public string Introduction { get; set; }
        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();
    }
}
