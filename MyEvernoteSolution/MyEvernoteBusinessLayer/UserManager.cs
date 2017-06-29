using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer//girilen kullanıcının bilgilerini kontrol et problem yoksa Insert yap
{
    public class UserManager
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();

        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            BusinessLayerResult<EvernoteUser> layer_result = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = repo_user.Find(x => x.Username == data.username || x.Email == data.email);//username yada mail daha önceden eklenmişse ilk if çalışıcak

            if (user!=null)
            {
                if(user.Username==data.username)
                {
                    layer_result.Errors.Add("Kullanıcı Adı daha önceden kaydedilmiş!");
                }
                if(user.Email==data.email)
                {
                    layer_result.Errors.Add("E-posta daha önceden kaydedilmiş!");
                }

            }
            else//bilgilerde problem yoksa insert yapılıcak yer 
            {
                int db_result = repo_user.Insert(new EvernoteUser()
                {
                    Username = data.username,
                    Email = data.email,
                    Password = data.password,
                    ActivateGuid=Guid.NewGuid(),// resim ekleyip tekrar dene  
                    ProfileImageFilename="user.png",
                    IsAdmin=false,
                    IsActive=false
                    //3 değişkeni insert fonk içinde otomatik ekliyo
                }
                );
                if (db_result>0)
                {
                    layer_result.Result = repo_user.Find(x => x.Email == data.email && x.Username == data.username);
                    //aktivasyon maili atılacak
                    //layer_result.Result.ActivateGuid
                }
            }
            return layer_result;//fonksiyonun dönüştipi EvernoteUser ! BLR olmadığı için aktarmak zorunda kaldık , BLR ta inserte bırakılan 3 değişken yok buda bi sebep
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<EvernoteUser> layer_result = new BusinessLayerResult<EvernoteUser>();
            layer_result.Result = repo_user.Find(x => x.Username == data.username && x.Password == data.password);

            if (layer_result.Result != null)
            {
                if(!layer_result.Result.IsActive)
                {
                    layer_result.Errors.Add("Kullanıcı aktifleştirilmedi! Lütfen mailinizi kontrol edin.");
                }

            }
            else
            {
                layer_result.Errors.Add("Kullanıcı Adı yada Şifre yanlış");
            }
            return layer_result;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = repo_user.Find(x => x.Id == id);

            if (res.Result==null)
            {
                res.Errors.Add("Kullanıcı Bulunamadı");
            }
            return res;
        }
    }
}
