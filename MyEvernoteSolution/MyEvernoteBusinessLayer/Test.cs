using MyEvernote.DataAccessLayer;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer
{
    public class Test
    {
        //Her kategori için CRUD işlemi yapıcak Repository objeleri oluşturuldu
        //bir fonksiyonu çağırıldığında repositorye gidip fonk çalıştırılmaz
        //bunların hepsi zaten repositorydir fonksiyonlar kendi fonksiyonları
        Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        Repository<Category> repo_cat = new Repository<Category>();
        Repository<Comment> repo_com = new Repository<Comment>();
        Repository<Note> repo_note = new Repository<Note>();
        Repository<Liked> repo_likee = new Repository<Liked>();
        public Test()
        {

            List<Category> at = repo_cat.List();
        }
        public void InsertTest()
        {

            int result = repo_user.Insert(new EvernoteUser
            {
                Name = "Fikibok",
                Surname = "Çükübik",
                Email = "m.emirosman@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "fikibik",
                Password = "123456",
                CratedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "fikibik"
            });
        }
        public void updateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id==15);
            if (user!=null)
            {
                user.Username = "AT";
            }
            repo_user.Update(user);
        }
        public void Delete()
        {
            EvernoteUser user = repo_user.Find(x => x.Id == 15);
            if (user!=null)
            {
                repo_user.Delete(user);
            }
        }
        public void CommentTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id == 3);
            Note note = repo_note.Find(x => x.Id == 3);
            Comment comment = new Comment()
            {
                Text = "deneme komment86",
                CratedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername="mistif",
                Note = note,
                Owner = user
            };
            repo_com.Insert(comment);
        }

      /*  public void likeTest()
        {
            Note note = repo_note.Find(x => x.Id == 58);
            EvernoteUser user = repo_user.Find(x => x.Id == 12);
            Liked like = new Liked()
            {
                Note = note,
                LikedUser = user
            };
            repo_likee.Insert(like);
        }*/
    }
} 
