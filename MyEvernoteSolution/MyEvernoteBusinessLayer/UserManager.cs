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
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Liked> repo_like = new Repository<Liked>();
        private Repository<Note> repo_note = new Repository<Note>();

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

        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser model)
        {
            BusinessLayerResult<EvernoteUser> update_result = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = repo_user.Find(x => x.Email == model.Email);

            user = repo_user.Find(x => (x.Email == model.Email && x.Id != model.Id));
            if(user!=null)
            {
                update_result.Errors.Add("Bu mail zaten kullanılıyo!");
                return update_result;
                
            }
            else
            {
                user = repo_user.Find(x => x.Id == model.Id);
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.Password = model.Password;
                user.ProfileImageFilename = model.ProfileImageFilename;
                repo_user.Update(user);
                update_result.Result = repo_user.Find(x => x.Id == user.Id) ;
            }

            

            return update_result;

        }
        public BusinessLayerResult<EvernoteUser> RemoveById(int id)
        {
            //Kullanıcıyı silmek için önce ilişkili olduğu notları yorumları beğenileri silip daha sonra kullanıcıyı siliyoruz
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            
            EvernoteUser user = repo_user.Find(x => x.Id == id);
            List<Comment> deleteComment = repo_comment.List(x => x.Owner.Id == id || x.Note.Owner.Id==id);//siliceğimiz kişinin yorumuysa veya siliceğimiz kişinin notuna yapılan yorumsa sil
            foreach(Comment sil in deleteComment)
            {
                if(repo_comment.Delete(sil)==0)
                {
                    res.Errors.Add("Yorum Silme işlemi başarısız!");
                    return res;
                }
            }
            List<Liked> deleteLike = repo_like.List(x => x.LikedUser.Id == id||x.Note.Owner.Id==id);//kullanıcının beğenisiyse yada kullanıcının notuna yapılan beğeniyse sil 
            foreach (Liked sil_like in deleteLike)
            {
                if (repo_like.Delete(sil_like) == 0)
                {
                    res.Errors.Add("Yorum Silme işlemi başarısız!");
                    return res;
                }
            }
            List<Note> deleteNote = repo_note.List(x => x.Owner.Id == id);//kullanıcının notuysa sil
            foreach (Note sil_note in deleteNote)
            {
                if(repo_note.Delete(sil_note)==0)
                {
                    res.Errors.Add("Yorum Silme işlemi başarısız!");
                    return res;
                }
            }
           
            if (user!=null)
            {
                if(repo_user.Delete(user)==0)//ilişkili veriler gittikten sonra kullanıcıyı sil 
                {
                    res.Errors.Add("Silme işlemi başarısız!");
                    return res;
                }
            }
            else
            {
                res.Errors.Add("Kullanıcı bulunamadı");
            }
            return res;
        }


    }
}
