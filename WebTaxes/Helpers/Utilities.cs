using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using WebTaxes.Models;


namespace WebTaxes.Helpers
{
    public class Utilities:IDisposable
    {
       

        private static ApplicationDbContext  userContext = new ApplicationDbContext();//Me conecto a las tables que usan los usuarios.

        private static WebTaxesContext  db = new WebTaxesContext();

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

        //Método paara enviar email a una cuenta
        public  static async Task SendMail(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(WebConfigurationManager.AppSettings["AdminUser"]);
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    //credenciales:
                    UserName = WebConfigurationManager.AppSettings["AdminUser"],
                    Password = WebConfigurationManager.AppSettings["AdminPassWord"],    
                };

                smtp.Credentials = credential;
                smtp.Host = WebConfigurationManager.AppSettings["SMTPName"];
                smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
                smtp.EnableSsl = true;

                await smtp.SendMailAsync(message);
            }

        }

        //Método paara enviar email a varias cuentas:
        //Método sobrecargado: con una lista de correos:
        public static async Task SendMail(List<string> mail, string subject, string body)
        {
            var message = new MailMessage();

            foreach (var to in mail)
            {
                message.To.Add(new MailAddress(to));
            }

            
            message.From = new MailAddress(WebConfigurationManager.AppSettings["AdminUser"]);
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    //credenciales:
                    UserName = WebConfigurationManager.AppSettings["AdminUser"],
                    Password = WebConfigurationManager.AppSettings["AdminPassWord"],
                };

                smtp.Credentials = credential;
                smtp.Host = WebConfigurationManager.AppSettings["SMTPName"];
                smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
                smtp.EnableSsl = true;

                await smtp.SendMailAsync(message);
            }

        }

        //Método Password Recovery:
        public static async Task PasswordRecovery(string email)
        {
            //busco el email, y garantizo que el email exista:
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);

            if (userASP == null)
            {
                return;
            }

            var user = db.TaxPaers.Where(tp=>tp.UserName == email).FirstOrDefault();

            if (user == null)
            {
                return;
            }

            var random = new Random();
            //var newPassword = ($"{user.FirsName.Trim().ToUpper().Substring(0,1)},{user.lastName.Trim().ToLower()}, {"2:04*"}{ random.Next(10000)}");
            var newPassword = string.Format("{0} {1} {2:04*}",
                               user.FirsName.Trim().ToUpper().Substring(0, 1),
                               user.lastName.Trim().ToLower(), 
                               random.Next(10000));

            userManager.RemovePassword(userASP.Id);
            userManager.AddPassword(userASP.Id, newPassword);

            var subject = "Taxes Password Recovery.";
            var body = string.Format(@"<h1>Taxes Password Recovery.</h1> 
                        <p>Your new password is:<strong>{0}</strong></p>
                        <p>Please change it for one, that you remember easyly.</p>"
                       , newPassword);

            //Aqui envio el correo con el nuevo password:
            await SendMail(email, subject, body);

        }

        //métdo cirra y abri la conexión:
        public void Dispose()
        {
            userContext.Dispose();

            //desconecto el contexto de datos:
            db.Dispose();
        }
    }
}