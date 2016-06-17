using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTaxes.Helpers;
using WebTaxes.Models;

namespace WebTaxes.Controllers
{

    public class TaxPaersController : Controller
    {
        private WebTaxesContext db = new WebTaxesContext();

       
        [HttpGet]
        [Authorize(Roles = "TaxPaer")]
        public ActionResult MyProperties()
        {
            //lo busco
            var view = db.TaxPaers.Where(tp=>tp.UserName == this.User.Identity.Name).FirstOrDefault();


            if (view  != null)
            {
                return View(view.Properties.ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult MySettings(TaxPaer view)
        {

            if (ModelState.IsValid)
            {
                db.Entry(view).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same Name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", view.DepartmentId);
                    ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", view.DocumentTypeId);
                    ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                                            OrderBy(m => m.Name), "MunicipalityId", "Name", view.MunicipalityId);



                    return View(view);
                }

                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", view.DepartmentId);
                ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", view.DocumentTypeId);
                ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                                        OrderBy(m => m.Name), "MunicipalityId", "Name", view.MunicipalityId);



                return RedirectToAction("Index", "Home");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", view.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", view.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                                    OrderBy(m => m.Name), "MunicipalityId", "Name", view.MunicipalityId);



            return View(view);
        }
        [HttpGet]
        [Authorize(Roles = "TaxPaer")]
        public ActionResult MySettings()
        {
            //Utilizo el find cuando se el id, utilizo el Where cuando lo tengo que buscar en la bd y tiene que existe
            //por que antes el hay un registro para utilizar la app:
            var view = db.TaxPaers.Where(tp => tp.UserName == this.User.Identity.Name).FirstOrDefault();

            //Si  existe:
            if (view != null)
            {

                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", view.DepartmentId);
                ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", view.DocumentTypeId);
                ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                                        OrderBy(m => m.Name), "MunicipalityId", "Name", view.MunicipalityId);


                return View(view);
            }

            //Si no  existe: lo envio al index del home:

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult DeleteProperty(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var view = db.Properties.Find(id);

            if (view == null)
            {
                return HttpNotFound();
            }

            db.Properties.Remove(view);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Error = "ERROR: " + ex.Message;
            }


            return RedirectToAction($"Details/{view.TaxPaerId}");
        }


        [HttpPost]
        public ActionResult EditProperty(Property view)
        {
            if (ModelState.IsValid)
            {
                db.Entry(view).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same Name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", view.DepartmentId);

                    ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                             OrderBy(m => m.Name), "MunicipalityId", "Name", view.DepartmentId);
                    ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(pt => pt.Description),
                                                            "PropertyTypeId", "Description", view.PropertyTypeId);

                    return View(view);
                }

                ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", view.DepartmentId);

                ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                         OrderBy(m => m.Name), "MunicipalityId", "Name", view.DepartmentId);
                ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(pt => pt.Description),
                                                        "PropertyTypeId", "Description", view.PropertyTypeId);

                return RedirectToAction($"Details/{view.TaxPaerId}");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", view.DepartmentId);

            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                     OrderBy(m => m.Name), "MunicipalityId", "Name", view.DepartmentId);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(pt => pt.Description),
                                                    "PropertyTypeId", "Description", view.PropertyTypeId);

            return View(view);


        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditProperty(int? propertyId, int? taxPaerId)
        {
            if (propertyId == null && taxPaerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var property = db.Properties.Find(propertyId);

            if (property == null)
            {
                return HttpNotFound();
            }

            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", property.DepartmentId);

            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == property.DepartmentId).
                                     OrderBy(m => m.Name), "MunicipalityId", "Name", property.DepartmentId);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(pt => pt.Description),
                                                    "PropertyTypeId", "Description", property.PropertyTypeId);

            return View(property);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddProperty(Property view)
        {
            if (true)
            {
                db.Properties.Add(view);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //ToDO: catch errors to improve the message
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "Ther are a record with the same description.");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);

                    }

                    ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", view.DepartmentId);

                    ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                             OrderBy(m => m.Name), "MunicipalityId", "Name", view.DepartmentId);
                    ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(pt => pt.Description),
                                                            "PropertyTypeId", "Description", view.PropertyTypeId);

                    return View(view);

                }

                return RedirectToAction($"Details/{view.TaxPaerId}");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", view.DepartmentId);

            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == view.DepartmentId).
                                     OrderBy(m => m.Name), "MunicipalityId", "Name", view.DepartmentId);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(pt => pt.Description),
                                                    "PropertyTypeId", "Description", view.PropertyTypeId);

            return View(view);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddProperty(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }



            var view = new Property
            {
                TaxPaerId = id.Value,//es valuess por que el valor del id es opcional:
            };

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.
                                                   Where(m => m.DepartmentId == db.Departments.FirstOrDefault().
                                                   DepartmentId).OrderBy(m => m.Name), "MunicipalityId", "Name");
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(pt => pt.Description), "PropertyTypeId", "Description");

            return View(view);
        }

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers
        public ActionResult Index()
        {
            var taxPaers = db.TaxPaers.Include(t => t.Department).Include(t => t.DocumentType).Include(t => t.Municipality);
            return View(taxPaers.ToList());
        }

        // GET: TaxPaers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxPaer taxPaer = db.TaxPaers.Find(id);
            if (taxPaer == null)
            {
                return HttpNotFound();
            }
            return View(taxPaer);
        }

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description");
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId).
                                                    OrderBy(m => m.Name), "MunicipalityId", "Name");
            return View();
        }

        // POST: TaxPaers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxPaerId,FirsName,lastName,UserName,Phone,Address,DepartmentId,MunicipalityId,DocumentTypeId,Document")] TaxPaer taxPaer)
        {
            if (ModelState.IsValid)
            {
                db.TaxPaers.Add(taxPaer);

                try
                {
                    db.SaveChanges();

                    Utilities.CeateUserASP(taxPaer.UserName, "TaxPaer");
                }
                catch (Exception ex)
                {


                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "Ther are a record with the same description.");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);

                    }

                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxPaer.DepartmentId);
                    ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
                    ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == taxPaer.DepartmentId).
                                                    OrderBy(m => m.Name), "MunicipalityId", "Name", taxPaer.MunicipalityId);
                    return View(taxPaer);
                }

                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxPaer.DepartmentId);
                ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
                ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == taxPaer.DepartmentId).
                                                    OrderBy(m => m.Name), "MunicipalityId", "Name", taxPaer.MunicipalityId);
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxPaer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == taxPaer.DepartmentId).
                                                    OrderBy(m => m.Name), "MunicipalityId", "Name", taxPaer.MunicipalityId);
            return View(taxPaer);
        }

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxPaer taxPaer = db.TaxPaers.Find(id);
            if (taxPaer == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxPaer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == taxPaer.DepartmentId).
                                                    OrderBy(m => m.Name), "MunicipalityId", "Name", taxPaer.MunicipalityId);
            return View(taxPaer);
        }

        // POST: TaxPaers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxPaerId,FirsName,lastName,UserName,Phone,Address,DepartmentId,MunicipalityId,DocumentTypeId,Document")] TaxPaer taxPaer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taxPaer).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                       && ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "Ther are a record with the same description.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxPaer.DepartmentId);
                    ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
                    ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == taxPaer.DepartmentId).
                                                    OrderBy(m => m.Name), "MunicipalityId", "Name", taxPaer.MunicipalityId);
                    return View(taxPaer);

                }

                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxPaer.DepartmentId);
                ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
                ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == taxPaer.DepartmentId).
                                                     OrderBy(m => m.Name), "MunicipalityId", "Name", taxPaer.MunicipalityId);
                return RedirectToAction("Index");

            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxPaer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId == taxPaer.DepartmentId).
                                                    OrderBy(m => m.Name), "MunicipalityId", "Name", taxPaer.MunicipalityId);
            return View(taxPaer);

        }

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxPaer taxPaer = db.TaxPaers.Find(id);
            if (taxPaer == null)
            {
                return HttpNotFound();
            }
            return View(taxPaer);
        }

        // POST: TaxPaers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaxPaer taxPaer = db.TaxPaers.Find(id);
            db.TaxPaers.Remove(taxPaer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetMunicipalities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var municipalities = db.Municipalities.
                Where(m => m.DepartmentId == departmentId).OrderBy(m => m.Name);
            return Json(municipalities);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
