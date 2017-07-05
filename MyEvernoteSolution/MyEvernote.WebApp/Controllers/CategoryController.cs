using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Entities;
using MyEvernoteBusinessLayer;

namespace MyEvernote.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryManager categorymanager = new CategoryManager();

        // GET: Category
        public ActionResult Index()
        {
            return View(categorymanager.List());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Category category =catagorymanager.GetCategoryById(id.Value);
            Category category = categorymanager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Category category)
        {
            if (ModelState.IsValid)
            {
                //db.Categories.Add(category);
                //db.SaveChanges();
                categorymanager.Insert(category);
                             
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x=>x.Id==id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit( Category category) //save de problem var !!!!
        {
            if (ModelState.IsValid)
            {
                Category cat = categorymanager.Find(x => x.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;
                categorymanager.Update(cat);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x=>x.Id==id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }//save de problem var

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categorymanager.Find(x => x.Id == id);
            categorymanager.Delete(category);
            return RedirectToAction("Index");
        }

    }
}
