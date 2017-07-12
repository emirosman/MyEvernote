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
        private CommentManager comm = new MyEvernoteBusinessLayer.CommentManager();
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

        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.user != null)
            {
            List<int> likedNoteIds = lm.List(x => x.LikedUser.Id == CurrentSession.user.Id && ids.Contains(x.Note.Id)).Select(x => x.Note.Id).ToList();
            return Json(new { result = likedNoteIds });
            }
            return Json(new { result = "" });
        }
        [HttpPost]
        public ActionResult setLikeState(int noteid, bool liked)
        {
            int res = 0;
            if (Session["login"] != null)
            {
                Liked like = lm.Find(x => x.Note.Id == noteid && x.LikedUser.Id == CurrentSession.user.Id);//bu kişi bu notu likelamışmı??
                Note note = notemanager.Find(x => x.Id == noteid);
                if (like != null && liked == false)//daha önce likelanmış / sil
                {
                    res = lm.Delete(like);
                }
                else if (like == null && liked == true)// yeni like oluştur
                {
                    Liked newlike = new Liked();
                    newlike.Note = note;
                    newlike.LikedUser = CurrentSession.user;
                    res = lm.Insert(newlike);
                }
                if (res > 0)
                {
                    if (liked)
                    {
                        note.LikeCount++;
                    }
                    else
                    {
                        note.LikeCount--;
                    }
                    res = notemanager.Update(note);
                    return Json(new { hasError = false, errorMessage = string.Empty, result = note.LikeCount });
                }
            }
            return Json(new { hasError = true, errorMessage = "Like işlemi başarısız, Giriş yapın!" });//sadece errorMessage döndürülüp count'u kontrol edilebilir ?
        }

        public ActionResult SendComment(int noteid, string comment)
        {
            if (comment == "")
            {
                return Json(new
                {
                    hasError = true,
                    errorMessage = "Yorum yazmadınız"
                });
            }
            int res = 0;
            Note note = notemanager.Find(x => x.Id == noteid);
            Comment new_com = new Comment();
            new_com.Note = note;
            new_com.Owner = CurrentSession.user;
            //new_com.ModifiedUsername = CurrentSession.user.Username;
            new_com.Text = comment;//boş geliyi ???
            res=comm.Insert(new_com);
            if(res>0)
            {
                return Json(new
                {
                    hasError = false,
                    errorMessage = string.Empty,
                    result_com = new_com.Text,
                    result_name = (new_com.Owner.Name + " " + new_com.Owner.Surname),
                    result_img= new_com.Owner.ProfileImageFilename

                });
            }
            else 
            return Json(new { hasError=true,
                    errorMesage="Yorum Yapılamadı!"
            });
        } 
    }
}
