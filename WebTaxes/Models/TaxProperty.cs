using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTaxes.Models
{
    public class TaxProperty
    {
        [Key]
        public int TaxPropertyId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int Year { get; set; }

       public DateTime  DateGenarated { get; set; }
       
        //? es para decir que el campo es opcional
        public DateTime? DatePay { get; set; }
        public bool IsPay { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 99999999999999, ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get; set; }

        //Relacion varios:
        public virtual Property Property{ get; set; }
    }
}