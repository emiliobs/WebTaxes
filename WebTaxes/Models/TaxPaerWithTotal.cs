using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTaxes.Models
{
    public class TaxPaerWithTotal
    {
        public TaxPaer TaxPaer { get; set; }

        //[Required(ErrorMessage = "You must enter a {0}")]
        //[Range(1, 99999999999999, ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Total { get; set; }
    }
}