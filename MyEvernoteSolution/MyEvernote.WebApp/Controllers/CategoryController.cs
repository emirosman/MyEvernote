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
using MyEvernoteBusinessLayer.Result;

namespace MyEvernote.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        BusinessLayerResult<Category> result = new BusinessLayerResult<Category>();
        CategoryManager cm = new CategoryManager();

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
        public ActionResult Create(Category category)//db de olan kategoriyi tekrar ekleyebiliyo kontrol et
        {
            
            if (ModelState.IsValid)
            {
                result = cm.create_cat(category);
                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(category);
                }
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
            //result ve cm yukarda tanımlandı
            
            if (ModelState.IsValid)
            {
                result = cm.update_cat(category);
                if(result.Errors.Count>0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x));//burda patlıyo olabilir???
                    return View(category);

                }
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
            BusinessLayerResult<Category> result = new BusinessLayerResult<Category>();
            Category delete_cat = categorymanager.Find(x => x.Id == id);
            if(ModelState.IsValid)
            {
                categorymanager.delete_cat(delete_cat);
                if(result.Errors.Count>0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(delete_cat);
                }
                return RedirectToAction("Index");
            }
            return View(delete_cat);
        }
    }
}
