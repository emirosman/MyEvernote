

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyIntializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Mıstıf",
                Surname = "Emiros",
                Email = "m.emirosman@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "emirosman",
                Password = "123456",
                ProfileImageFilename="user.png",
                CratedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "emirosman"
            };

            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = FakeData.NameData.GetFirstName(),
                Surname = FakeData.NameData.GetSurname(),
                Email = FakeData.NetworkData.GetEmail(),
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "fake",
                Password = "123456",
                ProfileImageFilename = "user.png",
                CratedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(8),
                ModifiedUsername = "fake"
            };
            context.EvernotUsers.Add(admin);
            context.EvernotUsers.Add(standartUser);
            for(int i=0;i<10;i++)
            {
                EvernoteUser User = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"fake{i}",
                    ProfileImageFilename = "user.png",
                    Password = "123456",
                    CratedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"fake{i}"
                };
               
                
                context.EvernotUsers.Add(User);
            }
            //silme kontrolü yapmak için yorumu beğenisi notu olmayan bi kullanıcı eklendi 
            EvernoteUser DeleteUser = new EvernoteUser()
            {
                Name = "Silinecek",
                Surname = "Kullanıcı",
                Email = FakeData.NetworkData.GetEmail(),
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "delete",
                Password = "123456",
                ProfileImageFilename = "user.png",
                CratedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(8),
                ModifiedUsername = "fake"
            };
            context.EvernotUsers.Add(DeleteUser);

            context.SaveChanges();
            List<EvernoteUser> userlist = context.EvernotUsers.ToList();

            //fake datalarla kategori yollanıcak
            for (int i=0;i<10;i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetCounty(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CratedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "emiros"
                };
                context.Categories.Add(cat);

                //oluşturulan kategoriye not girilicek
                for(int k=0;k<FakeData.NumberData.GetNumber(5,9);k++)
                {
                    EvernoteUser note_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner =note_owner,
                        ImagePath="resim"+ FakeData.NumberData.GetNumber(1, 10).ToString()+".jpg",
                        CratedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = note_owner.Username
                    };
                    cat.Notes.Add(note);
                    //oluşan nota yorum yapılıcak
                    for(int j=0;j<FakeData.NumberData.GetNumber(1,5);j++)
                    {
                        EvernoteUser comment_owner  = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            CratedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username,
                            Owner = comment_owner
                        };
                        note.Comments.Add(comment);
                    }

                    for( int m = 0;m < note.LikeCount;m++ )
                    {
                        
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]
                        };
                        note.Likes.Add(liked);
                        


                    }



                }
                
            }
            context.SaveChanges();
        }
    }
}
