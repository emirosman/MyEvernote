using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer
{
    public class CommentManager
    {
        public int DeleteComment(int id)
        {
            BusinessLayerResult<Comment> res = new BusinessLayerResult<Comment>();
            List<Comment> deleteCommentList = new List<Comment>();
            Repository<Comment> repoDeleteComment = new Repository<Comment>();
            deleteCommentList = repoDeleteComment.List(x => x.Owner.Id == id);
            foreach(Comment sil in deleteCommentList)
            {
                if (repoDeleteComment.Delete(sil) == 0)
                {
                    res.Errors.Add("yorum Silinemedi");
                    break;
                }
            }
            return 1;
        }
    }
}
