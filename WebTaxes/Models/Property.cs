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
        public int TaxPaerId { get; set; }
        public int DepartmentId { get; set; }
        public int MunicipalityId { get; set; }
        public int PropertyTypeId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Stratum { get; set; }
        public float Area { get; set; }

        //relacionnes: lado varios:
        public virtual TaxPaer TaxPaer { get; set; }

        public virtual Department Department { get; set; }

        public virtual Municipality Municipality { get; set; }

        public virtual PropertyType PropertyType { get; set; }

    }
}