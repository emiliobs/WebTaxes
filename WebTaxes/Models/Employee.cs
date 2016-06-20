using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebTaxes.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(30, ErrorMessage = "The field {0} can contain maximum {1} annd minimum {2} characters", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirsName { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(30, ErrorMessage = "The field {0} can contain maximum {1} and minimum {2} characters", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(50, ErrorMessage = "The field {0} can contain maximum {1} and minimum {2} characters", MinimumLength = 7)]
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        [Index("Employee_ UserName_Index", IsUnique = true)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(80, ErrorMessage = "The field {0} can contain maximum {1} and minimum {2} characters", MinimumLength = 7)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [StringLength(80, ErrorMessage = "The field {0} can contain maximum {1} and minimum {2} characters", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int MunicipalityId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]

        public int DocumentTypeId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(20, ErrorMessage = "The field {0} can contain maximum {1} and minimum {2} characters", MinimumLength = 5)]
        [Index("Employee_Document_Index", IsUnique = true)]
        [Display(Name = "Nº Document")]
        public string Document { get; set; }

        //[ForeignKey("EmployeeId")]
        public int? BossId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return ($"{this.FirsName}, {this.lastName}"); } }

        //lado varios de la relación:
        public virtual Department Department { get; set; }
        public virtual Municipality Municipality { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        //Relacion con ella misma:(tabla)
        public virtual Employee Boss { get; set; }
        public virtual List<Employee> Employees { get; set; }


    }
       
}