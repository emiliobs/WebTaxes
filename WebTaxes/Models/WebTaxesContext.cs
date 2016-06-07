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
        }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public System.Data.Entity.DbSet<WebTaxes.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<WebTaxes.Models.Municipality> Municipalities { get; set; }
    }
}
