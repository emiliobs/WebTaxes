using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTaxes.Models
{
    public   class Municipality
    {
        [Key]
        public int MunicipalityId { get; set; }
        public int DepartmentId { get; set; }

        [Required(ErrorMessage ="The field {0} is requered.")]
        [Index("Municipality_Name_index")]
        [StringLength(30,ErrorMessage ="The field {0} can contain maximun {1} and minimum {2} characters.", MinimumLength =3)]
        [Display(Name="Municipality")]
        public String Name { get; set; }

        //relacion varios:
        //con virtual no se va a la bd:
        public virtual Department Department { get; set; }
        public virtual ICollection<TaxPaer> TaxPaer { get; set; }
    }
}
