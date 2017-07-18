using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.Models;
using MyEvernoteBusinessLayer;
using MyEvernoteBusinessLayer.Result;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Net.Mail;
using GemBox.Spreadsheet;

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
            NoteManager notemanager = new NoteManager();
            var notes = notemanager.Find(x => x.Id == 1);
            return View(notes);
            //NoteManager nm = new NoteManager();
            //return View(nm.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());//orderby kısmını c# üstlenir
        }
        [HttpPost]
        public ActionResult indexdeneme(string pas)//dd.mm.yy
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("New Worksheet");
            worksheet.Cells["A1"].Value = "Hello World";
            workbook.Save("C:/Users/Emirosman/Desktop/mvc_app/MyEvernoteSolution/MyEvernote.WebApp/SpreadSheets/Hello.xls");
            return View();

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
                ///////////////////////////////////////////////yönlendirmeler yapılıcak!tekrar mail gönder seçeneği eklenicek! aktif olan tekrar mail isteyemesin
                try
                {                                                                                                                                  
                   SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");                                                                                 
                   var workbook = new ExcelFile();                                                                                                 
                   var worksheet = workbook.Worksheets.Add("Welcome");                                                                             
                   worksheet.Cells["A1"].Value = "User Name";                                                                                      
                   worksheet.Cells["A2"].Value = res.Result.Username;                                                                              
                   worksheet.Cells["B1"].Value = "Mail";                                                                                           
                   worksheet.Cells["B2"].Value = res.Result.Email;                                                                                 
                   worksheet.Cells["C1"].Value = "Aktivasyon Linki";                                                                               
                   worksheet.Cells["C2"].Value = "http://localhost:51560/Home/Activated/" + res.Result.ActivateGuid;                               
                   workbook.Save("C:/Users/Emirosman/Desktop/mvc_app/MyEvernoteSolution/MyEvernote.WebApp/SpreadSheets/" + res.Result.Id+".xls");  
                   /////excel oluşturucak bi yerde derli toplu bi hale gelsin//////////////////////////   
                   
                  // WebMail.SmtpServer = "smtp.gmail.com";//mail atmalık hesap aç!
                  // WebMail.EnableSsl = true;
                  // WebMail.UserName = "m.emirosman@gmail.com";
                  // WebMail.Password = "5348208314";
                  // WebMail.SmtpPort = 587;
                    //sabit bi yerde oluşturulsun ////////////////////////////////

                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("m.emirosman@gmail.com", "5348208314");
                    MailMessage mail = new MailMessage();
                    string fileName = "C:/Users/Emirosman/Desktop/mvc_app/MyEvernoteSolution/MyEvernote.WebApp/SpreadSheets/"+res.Result.Id+".xls";
                    SmtpServer.EnableSsl = true;
                    Attachment atch = new Attachment(fileName);
                    mail.From =new MailAddress( "m.emirosman@gmail.com");
                    mail.To.Add("m.emirosman@gmail.com" /*res.Result.Email*/);
                    mail.Subject = "My Evernote Aktivasyon";
                    mail.Body = "http://localhost:51560/Home/Activated/" + res.Result.ActivateGuid /*"<a href='http://localhost:51560/Home/Activated/" + res.Result.ActivateGuid + "'  ><b>tıkla</b> </a>"*/;
                    mail.Attachments.Add(atch);
                    SmtpServer.Send(mail);



                    //WebMail.Send(
                    //    /*"m.emirosman@gmail.com"*/res.Result.Email,
                    //    "MyEvernote Aktivasyon",
                    //    "<a href='http://localhost:51560/Home/Activated/"+res.Result.ActivateGuid+"'  ><b>tıkla</b> </a>",
                    //    "m.emirosman@gmail.com", null,filesToAttach:atch , true
                    //    );

                    return RedirectToAction("RegisterOk");

                }
                catch
                {
                    return RedirectToAction("indexdeneme");
                }

                
            }
            
            //aktivasyon e postası
            return View(model);
        }

        public ActionResult Activated(Guid? id)//null gelme eşleşmeyen gelme kontrolleri yapılıcak
        {
            if (id == null)
            {
                //activate kodu yok
            }
            UserManager um = new UserManager();
            EvernoteUser activatedUser = new EvernoteUser();
            activatedUser = um.Find(x => x.ActivateGuid == id );
            if (activatedUser == null)
            {
                //yanlış aktivasyon kodu
                return View("index");
            }
            else if (activatedUser.IsActive == true)
            {
                //kullanıcı zaten aktifleştirilmiş
                return View("index");
            }
            else
            {
                activatedUser.IsActive = true;
                um.Update(activatedUser);
                return RedirectToAction("ActivateOk");
            }
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult ActivateOk()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return  RedirectToAction("Index");
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
                model.IsActive = true;//profilimi düzenle sayfasına giriyosa aktiftir zaten kotnrol yapmaya gerek yok//Profili Sil Diye bi checkbox konulup orası kontol ettirilebilir belki
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
