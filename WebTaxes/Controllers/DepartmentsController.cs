using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTaxes.Models;

namespace WebTaxes.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private WebTaxesContext db = new WebTaxesContext();

        [HttpPost]
        public ActionResult EditMunicipality(Municipality view)
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

                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                       && ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same Name.");


                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    return View(view);
                }

                return RedirectToAction($"Details/{view.DepartmentId}");
            }

            return View(view);

           
        }

        [HttpGet]
        public ActionResult EditMunicipality(int? municipalityId, int? departmentId)
        {
            if (municipalityId == null && departmentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var municipality = db.Municipalities.Find(municipalityId);

            if (municipality == null)
            {
                return HttpNotFound();
            }

            return View(municipality);
        }

        [HttpGet]
        public ActionResult DeleteMunicipality(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var municipalty = db.Municipalities.Find(id);

            if (municipalty == null)
            {
                return HttpNotFound();
            }

            db.Municipalities.Remove(municipalty);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                ViewBag.Error = "ERROR: " + ex.Message;
                
            }

            //Borre o no el record si estando en la misma vista por que nunca sale de la vista details:
            return RedirectToAction($"Details/{municipalty.DepartmentId}");
        }

        [HttpPost]
        public ActionResult AddMunicipality(Municipality view)
        {

            if (ModelState.IsValid)
            {
                db.Municipalities.Add(view);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty,"There are a record with the same Name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    return View(view);
                }

                return RedirectToAction($"Details/{view.DepartmentId}");

            }

            return View(view);
        }
        public ActionResult AddMunicipality(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var department = db.Departments.Find(id);

            if (department == null)
            {
                return HttpNotFound();
            }

            var view = new Municipality
            {
                DepartmentId = department.DepartmentId,
            };

            return View(view);
        }

        // GET: Departments
        public ActionResult Index()
        {
            return View(db.Departments.OrderBy(d=>d.Name).ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Department department = db.Departments.Find(id);

            if (department == null)
            {
                return HttpNotFound();
            }


            

            //var view = new DepartmentView
                //{
                //    DepartmentId = department.DepartmentId,
                //    MunicipalityList = department.OrderBy(m=>m.Name).Municipalities.ToList(),
                //    Name = department.Name,
                //};

            //return View(view);
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentId,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("Index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same description.");


                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    return View(department);
                }

                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentId,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("Index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same description.");


                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    return View(department);


                }
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);

            db.Departments.Remove(department);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null && ex.InnerException.InnerException != null
                    && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty, "The record can't be delete because has related record.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
                return View(department);
            }

            return RedirectToAction("Index");
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
