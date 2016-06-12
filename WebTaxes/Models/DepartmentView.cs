using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebTaxes.Models
{
    //Atributo que inpide que esta clase envie datos a la base de datos:
    [NotMapped]
    public class DepartmentView : Department
    {
        public List<Municipality> MunicipalityList { get; set; }
    }
}