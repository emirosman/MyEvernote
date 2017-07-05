//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using MyEvernote.Entities;
//using MyEvernote.WebApp.Models;
//using MyEvernoteBusinessLayer;

//namespace MyEvernote.WebApp.Controllers
//{
//    public class NoteController : Controller
//    {
//        NoteManager notemanager = new NoteManager();
//        // GET: Note
//        public ActionResult Index()
//        {
            
//            var notes = db.Notes.Include(n => n.Category);
//            return View(notes.ToList());
//        }

//        // GET: Note/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Note note = db.Notes.Find(id);
//            if (note == null)
//            {
//                return HttpNotFound();
//            }
//            return View(note);
//        }

//        // GET: Note/Create
//        public ActionResult Create()
//        {
//            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Title");
//            return View();
//        }

//        // POST: Note/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,Title,Text,IsDraft,LikeCount,CategoryId,ImagePath,CratedOn,ModifiedOn,ModifiedUsername")] Note note)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Notes.Add(note);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Title", note.CategoryId);
//            return View(note);
//        }

//        // GET: Note/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Note note = db.Notes.Find(id);
//            if (note == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Title", note.CategoryId);
//            return View(note);
//        }

//        // POST: Note/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,Title,Text,IsDraft,LikeCount,CategoryId,ImagePath,CratedOn,ModifiedOn,ModifiedUsername")] Note note)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(note).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Title", note.CategoryId);
//            return View(note);
//        }

//        // GET: Note/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Note note = db.Notes.Find(id);
//            if (note == null)
//            {
//                return HttpNotFound();
//            }
//            return View(note);
//        }

//        // POST: Note/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Note note = db.Notes.Find(id);
//            db.Notes.Remove(note);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
