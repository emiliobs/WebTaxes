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
    public class MunicipalitiesController : Controller
    {
        private WebTaxesContext db = new WebTaxesContext();

        // GET: Municipalities
        public ActionResult Index()
        {
            var municipalities = db.Municipalities.Include(m => m.Department);
            return View(municipalities.OrderBy(m=>m.Name).ToList());
        }

        // GET: Municipalities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Municipality municipality = db.Municipalities.Find(id);

            if (municipality == null)
            {
                return HttpNotFound();
            }
            return View(municipality);
        }

        // GET: Municipalities/Create
        public ActionResult Create()
        {
            //aqui lo muetro e un comoboBox
            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d=>d.Name), "DepartmentId", "Name");

            return View();
        }

        // POST: Municipalities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MunicipalityId,DepartmentId,Name")] Municipality municipality)
        {
            if (ModelState.IsValid)
            {
                db.Municipalities.Add(municipality);
             

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                       && ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same description.");


                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
                    return View(municipality);
                }

                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
                return RedirectToAction("Index");

            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
            return View(municipality);

        }

        // GET: Municipalities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Municipality municipality = db.Municipalities.Find(id);
            if (municipality == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
            return View(municipality);
        }

        // POST: Municipalities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MunicipalityId,DepartmentId,Name")] Municipality municipality)
        {
            if (ModelState.IsValid)
            {
                db.Entry(municipality).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                       && ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same description.");


                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
                    return View(municipality);
                }

                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
            return View(municipality);
        }

        // GET: Municipalities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Municipality municipality = db.Municipalities.Find(id);
            if (municipality == null)
            {
                return HttpNotFound();
            }
            return View(municipality);
        }

        // POST: Municipalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Municipality municipality = db.Municipalities.Find(id);
            db.Municipalities.Remove(municipality);

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
                return View(municipality);
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
