using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using MyEvernoteBusinessLayer;
using MyEvernoteBusinessLayer.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        //new lenen satırları (managerlar) yukarı çekip tek tanımlamayla olayı kurtar
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
            return View(nm.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());//orderby kısmını c# üstlenir
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());//orderby kısmını sql yapar sorgu to.list dendiğinde çalıştırılır
        }

        /// ///////////////////////////////////////////////////////
        public ActionResult indexdeneme()
        {
            NoteManager nm = new NoteManager();
            return View(nm.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());//orderby kısmını c# üstlenir
        }
        ///////////////////////////////////////////////////////////

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryManager cm = new CategoryManager();
            Category cat = cm.Find(x=>x.Id==id.Value);
            if (cat == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index", nm.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
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
                res.Errors.ForEach(x => ModelState.AddModelError("", x));
                return View(res.Result);
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            EvernoteUser user = Session["login"] as EvernoteUser;
            UserManager eum = new UserManager();
            BusinessLayerResult<EvernoteUser> res = eum.GetUserById(user.Id);
            if (res.Errors.Count > 0)
            {
                //Hata ekranı verilicek
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model,HttpPostedFileBase ProfileImage)//hata yok ama kayıtlar güncellenmiyo debug debug bak
        {
            if (ModelState.IsValid)
            {
                //resim yüklemesi
                if ((ProfileImage != null) &&
                        (ProfileImage.ContentType == "image/jpeg" ||
                        ProfileImage.ContentType == "image/jpg" ||
                        ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ ProfileImage.ContentType.Split('/')[1]}";
                    model.ProfileImageFilename = filename;
                    string path = Path.Combine(Server.MapPath(("~/Images/Users/" + filename).ToString()));// başınsa / koyup dene!!!
                    ProfileImage.SaveAs(path);
                }
                UserManager eum = new UserManager();
                BusinessLayerResult<EvernoteUser> res = eum.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                    //hata kodları gelicek
                };
                Session["login"] = res.Result;
                return RedirectToAction("ShowProfile");
            }
            else
                return View(model);


        }

        public ActionResult RemoveProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            UserManager eum = new UserManager();
            BusinessLayerResult<EvernoteUser> res = eum.RemoveById(currentUser.Id);//gittiği fonksiyon önce kullanıcının ilişkili olduğu verileri sonra kullanıcıyı siliyo
            if(res.Errors.Count>0)//silinmediyse hataları bas
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x));
                return View(res);
            }
            Session.Clear();//silindiyse session ı boşalt
            return RedirectToAction("Index");
        }
    }
}
