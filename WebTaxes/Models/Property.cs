using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTaxes.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int TaxPaerId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int MunicipalityId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(20, ErrorMessage = "The field {0} can contain maximum {1} and minimum {2} characters", MinimumLength = 7)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(100, ErrorMessage = "The field {0} can contain maximum {1} and minimum {2} characters", MinimumLength = 10)]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 7, ErrorMessage ="The field {0} must be contain values between {1} and {2}")]
        public int Stratum { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 99999999, ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        public float Area { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 99999999999999, ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
         [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get; set; }

        //relacionnes: lado varios:
        public virtual TaxPaer TaxPaer { get; set; }

        public virtual Department Department { get; set; }

        public virtual Municipality Municipality { get; set; }

        public virtual PropertyType PropertyType { get; set; }

        public virtual ICollection<TaxProperty> TaxProperties { get; set; }

    }
}