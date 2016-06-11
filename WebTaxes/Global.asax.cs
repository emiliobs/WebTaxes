
using System.Data.Entity;
using WebTaxes.Migrations;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebTaxes.Models;
using System;
using WebTaxes.Helpers;

namespace WebTaxes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<WebTaxesContext, Configuration>());

            //método para mirarlos roles si existes, si no lo crea:
            this.CheckRoles();

            //método para mirar o crear el super usuario(admin):
            Utilities.CheckSuperUser();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        

        public void CheckRoles()
        {
            Utilities.CheckRole("Admin");
            Utilities.CheckRole("Employee");
            Utilities.CheckRole("TaxPaer");
        }
    }
}
