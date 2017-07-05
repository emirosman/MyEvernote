using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Entities;
using MyEvernote.WebApp.Models;
using MyEvernoteBusinessLayer;
using MyEvernote.Entities.ValueObjects;
using MyEvernoteBusinessLayer.Result;

namespace MyEvernote.WebApp.Controllers
{
    public class UserController : Controller
    {
        BusinessLayerResult<EvernoteUser> result = new BusinessLayerResult<EvernoteUser>();
        UserManager um = new UserManager();
        // GET: User
        public ActionResult Index()
        {
            return View(um.List());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = um.Find(x=>x.Id==id.Value);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(EvernoteUser evernoteUser )
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                result=um.CreateUser(evernoteUser);
                if(result.Errors.Count>0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(evernoteUser);
                }
                return RedirectToAction("Index");
            }
            return View(evernoteUser);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EvernoteUser evernoteUser =um.Find(x=>x.Id==id);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EvernoteUser evernoteUser)
        {
            if (ModelState.IsValid)
            {
                //user manager içindeki update kullanıldı çünkü bu kayıt daha önceden yapılmış uyarısı orda zaten veriliyodu burda tekrar yazmaya gerek yok
                //EvernoteUser update_user = new EvernoteUser();
                //update_user = um.Find(x => x.Id == evernoteUser.Id);
                //update_user.Email = evernoteUser.Email;
                //update_user.IsActive = evernoteUser.IsActive;
                //update_user.IsAdmin = evernoteUser.IsAdmin;
                //update_user.Name = evernoteUser.Name;
                //update_user.Password = evernoteUser.Password;
                //update_user.ProfileImageFilename = evernoteUser.ProfileImageFilename;
                //update_user.Surname = evernoteUser.Surname;
                result = um.UpdateProfile(evernoteUser);
                if(result.Errors.Count>0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(evernoteUser);
                }
                
                return RedirectToAction("Index");
            }
            return View(evernoteUser);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser =um.Find(x=>x.Id==id.Value);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessLayerResult<EvernoteUser> delete_result = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser evernoteUser = um.Find(x => x.Id == id);
            delete_result = um.RemoveById(evernoteUser.Id);
            if(delete_result.Errors.Count>0)
            {
                delete_result.Errors.ForEach(x => ModelState.AddModelError("", x));
                return View(result);
            }
            //um.Delete(evernoteUser);//delete direk repositorydeki fonksiyona gider bunun yerine usermanagerdaki removebyid çağır ilişkili olduğu satırlarıda silsin 

            return RedirectToAction("Index");
        }
    }
}
