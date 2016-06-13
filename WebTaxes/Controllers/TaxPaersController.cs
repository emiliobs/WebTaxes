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

        //Todo: here going:
        [HttpGet]
        [Authorize(Roles = "TaxPaer")]
        public ActionResult MySettings()
        {
            return View();
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

            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d=>d.Name), "DepartmentId", "Name", view.DepartmentId);

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

            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            

            var view = new Property
            {
                TaxPaerId = id.Value,//es valuess por que el valor del id es opcional:
            };

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.
                                                   Where(m=>m.DepartmentId == db.Departments.FirstOrDefault().
                                                   DepartmentId).OrderBy(m=>m.Name), "MunicipalityId", "Name");
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
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m => m.DepartmentId ==  db.Departments.FirstOrDefault().DepartmentId).
                                                    OrderBy(m=>m.Name), "MunicipalityId", "Name");
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
                catch (Exception ex)               {


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
            ViewBag.MunicipalityId = new SelectList(db.Municipalities.Where(m=>m.DepartmentId == taxPaer.DepartmentId).
                                                    OrderBy(m=>m.Name), "MunicipalityId", "Name", taxPaer.MunicipalityId);
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
                Where(m => m.DepartmentId == departmentId).OrderBy(m =>m.Name);
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
