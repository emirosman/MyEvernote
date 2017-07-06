using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernoteBusinessLayer.Abstract;
using MyEvernoteBusinessLayer.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer
{
    public class NoteManager : ManagerBase<Note>
    {
        private Repository<Note> repo_note = new Repository<Note>();//kullanıldığı yerleri bul/ yeni sisteme geçir/ bu satırı sil

        public BusinessLayerResult<Note> remove_note(int id)//içindeki like ve yorumlarla birlikte notu siler
        {
            BusinessLayerResult<Note> result = new BusinessLayerResult<Note>();
            LikedManager lm = new LikedManager();
            CommentManager cm = new CommentManager();

            List<Liked> delete_liked = lm.List(x => x.Note.Id == id);
            foreach (Liked sil_l in delete_liked)
            {
                if (lm.Delete(sil_l) == 0)
                {
                    result.Errors.Add("Like silme başarısız");
                    return result;
                }
            }
            List<Comment> delete_comment = cm.List(x => x.Note.Id == id);
            foreach (Comment sil_c in delete_comment)
            {
                if (cm.Delete(sil_c) == 0)
                {
                    result.Errors.Add("Yorum silme başarısız");
                    return result;
                }
            }

            if (Delete(Find(x=>x.Id==id)) == 0)
            {
                result.Errors.Add("Not silme başarısız");
            }
            return result;
        }

    }
}

