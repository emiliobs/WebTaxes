using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTaxes.Models
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [Index("DocumentType_Description_Index", IsUnique = true)]
        [StringLength(30, ErrorMessage = "The field {0} can contain maximum {1} and minimum {2} characters.", MinimumLength = 3)]
        [Display(Name = "Document Type")]
        public string Description { get; set; }

        public virtual ICollection<TaxPaer> TaxPaer { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }

       
}