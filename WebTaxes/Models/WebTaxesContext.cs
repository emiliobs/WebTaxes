using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WebTaxes.Models
{
    public class WebTaxesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public WebTaxesContext() : base("name=DefaultConnection")
        {
        }

        //Dabilitar borrado en cascada:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Employee>()
                        .HasOptional(x => x.Boss)
                        .WithMany(x => x.Employees)
                        .HasForeignKey(x => x.BossId);
        }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Municipality> Municipalities { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public System.Data.Entity.DbSet<WebTaxes.Models.TaxPaer> TaxPaers { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TaxProperty> TaxProperties { get; set; }

        public System.Data.Entity.DbSet<WebTaxes.Models.Employee> Employees { get; set; }
    }
}
