using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WebTaxes.Models;

namespace WebTaxes.Helpers
{
    public class Utilities:IDisposable
    {
        private static ApplicationDbContext  userContext = new ApplicationDbContext();//Me conecto a las tables que usan los usuarios.
        public static void CheckRole(string roleName)
        {
            //pasos para crear los roles:
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));


            //Check to see if Role Exists, if not create it.
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }


        }

        //Métdo garantiza que hay un super usuario administrador:
        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassWord"];
            //Busco el susuario en la DB y pregunto si existe:
            var userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                CeateUserASP(email, "Admin", password);
                return;
            }

            //Si existe le agrego el role admin:
            userManager.AddToRole(userASP.Id,"Admin");

        }

        //sin el parametro password
        //crea los usuarios en la tabla AspNetUserRoles
        //método generico(parametros)
        public static void CeateUserASP(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            //pasos para crear el usuario
            //creo el usuario con el role:
            //create the ASP NET User:
            var userASP = new ApplicationUser
            {
                 Email = email,
                 UserName = email,
            };

            userManager.Create(userASP,email);

            //Add user to role:
            userManager.AddToRole(userASP.Id, roleName);
        }
    
        //con password
        public static void CeateUserASP(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            //pasos para crear el usuario
            //creo el usuario con el role:
            //create the ASP NET User:
            var userASP = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            userManager.Create(userASP, password);

            //Add user to role:
            userManager.AddToRole(userASP.Id, roleName);
        }

        //métdo cirra y abri la conexión:
        public void Dispose()
        {
            userContext.Dispose();
        }
    }
}