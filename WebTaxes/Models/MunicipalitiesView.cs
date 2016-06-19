using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTaxes.Models
{
    public class MunicipalitiesView
    {
        //public int MunicipalitiesViewId { get; set; }
        public string Department { get; set; }
        public string Municipality { get; set; }

        public List<Municipality> Municipalities { get; set; }
    }
}