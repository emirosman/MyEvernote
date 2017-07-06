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

namespace MyEvernote.WebApp.Controllers
{
    public class NoteController : Controller
    {
        private CategoryManager cm = new CategoryManager();
        private NoteManager notemanager = new NoteManager();
        private LikedManager lm = new LikedManager();

        // GET: Note
        public ActionResult Index()
        {

            //listelerken kategori tablosunu listelemeye kat=> parametre olarak entitydeki visual değişkeni göster tablo adını değil                 //değiştirilme tarihine göre sırala
            var notes = notemanager.ListQueryable().Include("Category").Include("Owner").Where(x => x.Owner.Id == CurrentSession.user.Id).OrderByDescending(x => x.ModifiedOn);
            return View(notes.ToList());//yukarıda sorgu oluşturuldu to list dendiğinde çalıştırıldı
        }

        // GET: Note/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x=>x.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Note/Create
        public ActionResult Create()
        {


            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title");
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            note.Owner = CurrentSession.user;
            if (note.ImagePath == null)//eklenicek defoultları insertin içinde toplamayı dene?????
                note.ImagePath = "defoult.jpg";
            if (ModelState.IsValid)
            {
                notemanager.Insert(note);
                return RedirectToAction("Index");
            }
            //categori listesi sürekli çekiliyo, değişken bi liste değil ileride cashe alınıp cash ten çekilicek
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x=>x.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                Note update_note = notemanager.Find(x => x.Id == note.Id);
                update_note.IsDraft = note.IsDraft;
                update_note.Text=note.Text;
                update_note.Title=note.Title;
                update_note.CategoryId = note.CategoryId;
                update_note.ImagePath = note.ImagePath;

                notemanager.Update(update_note);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            notemanager.remove_note(id);//hata kontrol koyulabilir
            return RedirectToAction("Index");
        }

        public ActionResult LikeIndex()
        {
            var like = lm.ListQueryable().Include("Note").Include("LikedUser").Where(x => x.LikedUser.Id == CurrentSession.user.Id);
            return View(like.ToList());
        }
    }
}
