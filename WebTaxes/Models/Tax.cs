using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTaxes.Models
{
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 7, ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        public int Stratum { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(0, 1, ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public float Rate { get; set; }

    }
}