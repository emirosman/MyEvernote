using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using MyEvernoteBusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //MyEvernoteBusinessLayer.Test test = new MyEvernoteBusinessLayer.Test();
            //test.CommentTest();


            //categoryController üzerinden gelen view talebi
            //if(TempData["catNote"]!=null)
            //{
            //    return View(TempData["catNote"]as List<Note>);
            //}

            NoteManager nm = new NoteManager();
            return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());//orderby kısmını c# üstlenir
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());//orderby kısmını sql yapar sorgu to.list dendiğinde çalıştırılır
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);
            if (cat == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index", nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                 UserManager eum = new UserManager();
                 BusinessLayerResult<EvernoteUser> res=eum.LoginUser(model);
                
                 if(res.Errors.Count > 0)
                 {
                     res.Errors.ForEach(x => ModelState.AddModelError("", x));
                     return View(model);
                 }

                Session["login"] = res.Result;
                return RedirectToAction("Index");
            }
            
            //giriş kontrolü ve yönlendirme
            //session kullanıcı bilgileri
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)//textboxlardaki kurallara uyuyo //değil sanırım onu modelin errorlarıyla kontrol ettik
            {
                UserManager eum = new UserManager();
                BusinessLayerResult<EvernoteUser> res = eum.RegisterUser(model);//Hata varsa hata mesajlarını döndürüp if içine giricek yoksa Insert yapıp registerOk yönlenicek
                if(res.Errors.Count >0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);                  
                }


                return RedirectToAction("RegisterOk");
            }
            //kullanıcı username kontrolü
            //kullanıcı e posta kontrolü
            //kayıt işlemi
            //aktivasyon e postası
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return  RedirectToAction("Index");
        }

        public ActionResult UserActivate(Guid activate_id)
        {
            //kullanıcı aktivasyonu
            return View();

        }

        public ActionResult ShowProfile()
        {
            EvernoteUser user = Session["login"] as EvernoteUser;
            UserManager eum = new UserManager();
            BusinessLayerResult<EvernoteUser> res = eum.GetUserById(user.Id);
            if(res.Errors.Count>0)
            {
                //Hata ekranı verilicek
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser user)
        {
            return View();
        }

        public ActionResult RemoveProfile()
        {
            return View();
        }

    }
}
